using System;

namespace GroceryStore.PoS.Runner.Utils.DI
{
    public class RegistrationEntry
    {
        public Type InterfaceType { get; set; }
        public Type ImplementationType { get; set; }
        public InstanceLifeTimeMode LifeTimeMode { get; set; } = InstanceLifeTimeMode.Singleton;

        public object ImplementationInstance { get; set; }
        public Func<object> ImplementationInstanceFunc { get; set; }

        public object SimpleResolve()
        {
            return ImplementationInstance ?? ImplementationInstanceFunc?.Invoke();
        }
    }
}
