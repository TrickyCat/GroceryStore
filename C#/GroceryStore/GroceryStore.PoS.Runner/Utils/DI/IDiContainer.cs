using System;

namespace GroceryStore.PoS.Runner.Utils.DI
{
    public interface IDiContainer
    {
        void Clear();

        RegistrationEntry Register(Type interfaceType, Type implementationType = null);
        RegistrationEntry Register(Type interfaceType, object implementation);
        RegistrationEntry Register(Type interfaceType, Func<object> implementationFn);
        RegistrationEntry Register<TInterface, TImplementation>();
        RegistrationEntry Register<T>(Type implementationType = null);
        RegistrationEntry Register<T>(object implementation);
        RegistrationEntry Register<T>(Func<object> implementationFn);


        T Resolve<T>();
        object Resolve(Type requestedType);
    }
}
