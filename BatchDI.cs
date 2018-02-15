namespace BatchDI.AspNetCore
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    /**@Usage
    service.BatchInject(
        filter: "*Service",
        blacklist: new[] { "NoSingletonService", "AnotherService" },
        injector: _class => service.AddSingleton<_implementation>()
    );
    service.BatchInject(
        filter: new[] { "*Service", "*Controllers" },
        injector: _class => service.AddSingleton<_implementation>()
        blacklist: new[] { "NoSingletonService", "AnotherService" },
    );
    service.BatchInject(
        injector: (_interface, _class) => service.AddSingleton<_interface, _class>()
        filter: "*Service",
        blacklist: new[] { "NoSingletonService", "AnotherService" },
    );
     */

    public static class BatchDI
    {
        private static void BatchInjector(Delegate injectionFunc, string filter, string[] blacklist = null)
        {
            Func<Type, bool> filterClassName;
            if (filter.StartsWith("*"))
                filterClassName = t => t.Name.EndsWith(filter.Replace("*", ""));
            else if (filter.EndsWith("*"))
                filterClassName = t => t.Name.StartsWith(filter.Replace("*", ""));
            else
                filterClassName = t => t.Name.Contains(filter.Split('*')[0]) || t.Name.Contains(filter.Split('*')[1]);

            var types = Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t =>
                    {
                        try
                        {
                            // @see https://stackoverflow.com/a/18485574/5221998
                            return t.Namespace.StartsWith(Assembly.GetCallingAssembly().EntryPoint.DeclaringType.Namespace) && filterClassName(t);
                        }
                        catch (System.NullReferenceException)
                        {
                            return false;
                        }
                    });

            if (!filter.StartsWith("*") || !filter.EndsWith("*"))
            {
                var grouptypes = from t in types
                                 group t by t.Name.Replace(filter.Split('*')[1], "").Replace(filter.Split('*')[0], "") into tGroup
                                 where tGroup.Count() == 2
                                 select tGroup.ToArray();

                foreach (var group in grouptypes)
                {
                    Type _interface = group.Where(x => x.IsInterface).First();
                    Type _implementation = group.Where(x => _interface.IsAssignableFrom(x)).First();
                    // _services.AddSingleton(_interface, _implementation);
                    injectionFunc.DynamicInvoke(_interface, _implementation);
                }
            }
            else foreach (var implementation in types) injectionFunc.DynamicInvoke(implementation);
        }

        public static void BatchInject(this IServiceCollection services, Action<Type, Type> injector, string filter, string[] blacklist = null)
        {
            BatchInjector(injector, filter, blacklist);
        }

        public static void BatchInject(this IServiceCollection services, Action<Type> injector, string filter, string[] blacklist = null)
        {
            BatchInjector(injector, filter, blacklist);
        }

        public static void BatchInject(this IServiceCollection services, Action<Type, Type> injector, string[] filter, string[] blacklist = null)
        {
            foreach (var f in filter) BatchInjector(injector, f, blacklist);
        }

        public static void BatchInject(this IServiceCollection services, Action<Type> injector, string[] filter, string[] blacklist = null)
        {
            foreach (var f in filter) BatchInjector(injector, f, blacklist);
        }
    }
}
