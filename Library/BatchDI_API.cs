using System;
using System.Reflection;

namespace BatchDI
{
    public partial class BatchDI
    {
        public static void BatchInject(Action<Type, Type> injector, dynamic filter, dynamic blacklist = null, bool parallel = false)
        {
            inject(injector, filter, blacklist, parallel);
        }

        public static void BatchInject(Action<Type> injector, dynamic filter, dynamic blacklist = null, bool parallel = false)
        {
            inject(injector, filter, blacklist, parallel);
        }

        public static void SetEntryAssembly(Assembly assembly) => EntryAssembly = assembly;

        private static void inject(dynamic caller, dynamic filter, dynamic blacklist, bool parallel)
        {
            if (filter is string) BatchInjector(caller, filter, blacklist, parallel);
            else if (filter is string[]) foreach (var f in filter) BatchInjector(caller, f, blacklist, parallel);
            else throw new System.ArgumentException($"{nameof(filter)} must be `string` or `string[]`");
        }
    }
}