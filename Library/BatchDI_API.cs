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
                                        bool includeNestedClass = true
                                        )
        {
            inject(injector, filter, blacklist, parallel, includeNestedClass);
        }

        // only class
        public static void BatchInject(Action<Type> injector, dynamic filter,
                                        dynamic blacklist = null,
                                        bool parallel = false,
                                        bool includeNestedClass = true
                                        )
        {
            inject(injector, filter, blacklist, parallel, includeNestedClass);
        }

        public static void SetEntryAssembly(Assembly assembly) => EntryAssembly = assembly;

        private static void inject(dynamic caller, dynamic filter, dynamic blacklist, bool parallel, bool includeNestedClass)
        {
            if (filter is string) BatchInjector(caller, filter, blacklist, parallel, includeNestedClass);
            else if (filter is string[]) foreach (var f in filter) BatchInjector(caller, f, blacklist, parallel, includeNestedClass);
            else throw new System.ArgumentException($"{nameof(filter)} must be `string` or `string[]`");
        }
    }
}