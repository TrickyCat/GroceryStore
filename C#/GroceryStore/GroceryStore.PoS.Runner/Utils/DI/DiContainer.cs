using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GroceryStore.PoS.Runner.Utils.DI
{
    public class DiContainer : IDiContainer
    {
        private readonly Stack<Type> _currentResolutionStack = new Stack<Type>();
        private readonly Dictionary<Type, object> _instancesCache = new Dictionary<Type, object>();
        private readonly Dictionary<Type, RegistrationEntry> _registrationsCache = new Dictionary<Type, RegistrationEntry>();

        #region Public API

        public void Clear()
        {
            _registrationsCache.Clear();
        }

        public RegistrationEntry Register(Type interfaceType, Type implementationType = null)
        {
            if (interfaceType == null)
            {
                throw new ArgumentNullException(nameof(interfaceType));
            }
            if (implementationType == null)
            {
                implementationType = interfaceType;
            }
            var regEntry = new RegistrationEntry
            {
                InterfaceType = interfaceType,
                ImplementationType = implementationType
            };
            _registrationsCache[interfaceType] = regEntry;
            return regEntry;
        }

        public RegistrationEntry Register(Type interfaceType, object implementation)
        {
            if (interfaceType == null)
            {
                throw new ArgumentNullException(nameof(interfaceType));
            }
            if (implementation == null)
            {
                throw new ArgumentNullException(nameof(implementation));
            }
            var regEntry = new RegistrationEntry
            {
                InterfaceType = interfaceType,
                ImplementationInstance = implementation
            };
            _registrationsCache[interfaceType] = regEntry;
            return regEntry;
        }

        public RegistrationEntry Register(Type interfaceType, Func<object> implementationFn)
        {
            if (interfaceType == null)
            {
                throw new ArgumentNullException(nameof(interfaceType));
            }
            if (implementationFn == null)
            {
                throw new ArgumentNullException(nameof(implementationFn));
            }
            var regEntry = new RegistrationEntry
            {
                InterfaceType = interfaceType,
                ImplementationInstanceFunc = implementationFn
            };
            _registrationsCache[interfaceType] = regEntry;
            return regEntry;
        }

        public RegistrationEntry Register<TInterface, TImplementation>()
        {
            return Register(typeof(TInterface), typeof(TImplementation));
        }

        public RegistrationEntry Register<T>(Type implementationType = null)
        {
            return Register(typeof(T), implementationType);
        }

        public RegistrationEntry Register<T>(object implementation)
        {
            return Register(typeof(T), implementation);
        }

        public RegistrationEntry Register<T>(Func<object> implementationFn)
        {
            return Register(typeof(T), implementationFn);
        }

        public T Resolve<T>()
        {
            var requestedType = typeof(T);
            return (T)Resolve(requestedType);
        }

        public object Resolve(Type requestedType)
        {
            return ResolveImplWithStack(requestedType);
        }

        #endregion

        private object ResolveImplWithStack(Type requestedType)
        {
            if (_currentResolutionStack.Contains(requestedType))
            {
                _currentResolutionStack.Push(requestedType);
                throw new ArgumentException(
                    "Cyclic dependency\n" +
                    $"{ string.Join(" -> ", _currentResolutionStack.Reverse().Select(t => t.Name)) }"
                    );
            }
            _currentResolutionStack.Push(requestedType);
            var result = ResolveImpl(requestedType);
            _currentResolutionStack.Pop();
            return result;
        }


        private object ResolveImpl(Type requestedType)
        {
            if (requestedType == null)
            {
                throw new ArgumentNullException(nameof(requestedType));
            }
            var instance = GetFromCache(requestedType);
            if (instance != null)
            {
                return instance;
            }
            var implementationType = _registrationsCache.ContainsKey(requestedType)
                            ? _registrationsCache[requestedType]?.ImplementationType
                            : requestedType;
            var ctors = GetCtors(implementationType);
            ctors = SortCtors(ctors);
            var theChosenOne = ctors
                .Select(ctor => new { ctor, parameters = SatisfyCtorParams(ctor) })
                .FirstOrDefault(r => r.parameters != null);
            if (theChosenOne != null)
            {
                instance = theChosenOne.ctor.Invoke(theChosenOne.parameters);
                OnResolveSucess(requestedType, instance);
                return instance;
            }
            throw new ArgumentException($"Unable to instantiate the type [{ requestedType.FullName }]");
        }

        private void OnResolveSucess(Type requestedType, object instance)
        {
            if (_registrationsCache.ContainsKey(requestedType))
            {
                var regEntry = _registrationsCache[requestedType];
                if (regEntry.LifeTimeMode == InstanceLifeTimeMode.Singleton && !_instancesCache.ContainsKey(requestedType))
                {
                    _instancesCache[requestedType] = instance;
                }
            }
        }

        #region Cache

        private object GetFromCache(Type requestedType)
        {
            var instance = GetFromInstancesCache(requestedType);
            return instance ?? GetFromRegistrationsCache(requestedType);
        }

        private object GetFromInstancesCache(Type requestedType)
        {
            if (_registrationsCache.ContainsKey(requestedType))
            {
                var regEntry = _registrationsCache[requestedType];
                if (regEntry.LifeTimeMode == InstanceLifeTimeMode.Singleton && _instancesCache.ContainsKey(requestedType))
                {
                    return _instancesCache[requestedType];
                }
            }
            return null;
        }

        private object GetFromRegistrationsCache(Type requestedType)
        {
            if (_registrationsCache.ContainsKey(requestedType))
            {
                var regEntry = _registrationsCache[requestedType];
                var instance = regEntry.SimpleResolve();
                return instance;
            }
            return null;
        }
        #endregion


        private IEnumerable<ConstructorInfo> GetCtors(Type type)
        {
            return type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
        }

        private IEnumerable<ConstructorInfo> SortCtors(IEnumerable<ConstructorInfo> ctors)
        {
            return ctors.OrderBy(c => c.GetParameters().Length);
        }

        private object[] SatisfyCtorParams(ConstructorInfo ctor)
        {
            return ctor.GetParameters()
                .Select(paramInfo =>
                {
                    var paramType = paramInfo.ParameterType;
                    var paramInstance = ResolveImplWithStack(paramType);
                    return paramInstance;
                })
                .ToArray();
        }
    }
}
