using System;
using System.Reflection;

namespace BatchDI
{
    public partial class BatchDI
    {
        // with interface
        public static void BatchInject(Action<Type, Type> injector, dynamic filter,
                                        dynamic blacklist = null,
                                        bool parallel = false,
                                        bool nested = true
                                        )
        {
            inject(injector, filter, blacklist, parallel, nested);
        }

        // only class
        public static void BatchInject(Action<Type> injector, dynamic filter,
                                        dynamic blacklist = null,
                                        bool parallel = false,
                                        bool nested = true
                                        )
        {
            inject(injector, filter, blacklist, parallel, nested);
        }

        public static void SetEntryAssembly(Assembly assembly) => EntryAssembly = assembly;

        private static void inject(dynamic caller, dynamic filter, dynamic blacklist, bool parallel, bool nested)
        {
            if (filter is string) BatchInjector(caller, filter, blacklist, parallel, nested);
            else if (filter is string[]) foreach (var f in filter) BatchInjector(caller, f, blacklist, parallel, nested);
            else throw new System.ArgumentException($"{nameof(filter)} must be `string` or `string[]`");
        }
    }
}