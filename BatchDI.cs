using System;
using System.Linq;
using System.Reflection;

namespace AspNet.DependencyInjection.Batch
{
    public static partial class BatchDependencyInjectionExtensions
    {
        private static bool hasInterface(string filter)
        {
            return (!filter.StartsWith("*") && !filter.EndsWith("*")) ||
                    (filter.StartsWith("*") && filter.EndsWith("*"));
        }

        private static void inject(dynamic caller, dynamic filter, dynamic blacklist)
        {
            if (filter is string) BatchInjector(caller, filter, blacklist);
            else if (filter is string[]) foreach (var f in filter) BatchInjector(caller, f, blacklist);
            else throw new System.ArgumentException($"{nameof(filter)} must be `string` or `string[]`");
        }

        private static void BatchInjector(Delegate injector, string filter, dynamic blacklist = null)
        {
            /** @Check filter and blacklist string
            create helper for checking if its array, string glob patter, or just string */
            bool notInBlacklist(string className)
            {
                if (blacklist is string[])
                {
                    Func<string, bool> check = el => el == className;
                    return Array.Exists(blacklist, check);
                }
                else if (blacklist is string)
                {
                    if (blacklist.Contains("*")) return className.Contains(blacklist.Replace("*", ""));
                    else return blacklist == className;
                }
                else if (blacklist == null) { return true; }
                else throw new System.ArgumentException($"{nameof(blacklist)} must be `string` or `string[]`");
            }
            Func<string, bool> filterClassName;
            if (filter.StartsWith("*")) filterClassName = t => t.EndsWith(filter.Replace("*", ""));
            else if (filter.EndsWith("*")) filterClassName = t => t.StartsWith(filter.Replace("*", ""));
            else filterClassName = t =>
            {
                if (filter.Contains("*")) return t.Contains(filter.Split('*')[0]) || t.Contains(filter.Split('*')[1]);
                else return t == filter;
            };
            /** @ENDCheck filter and blacklist string */

            // Filter based on namespace and rule above
            var types = Assembly.GetEntryAssembly()
                    .GetTypes()
                    .Where(t => t.Namespace.StartsWith(Assembly.GetEntryAssembly().EntryPoint.DeclaringType.Namespace ?? "") &&
                                filterClassName(t.Name) && notInBlacklist(t.Name)
            );

            /** @Implementation */
            if (hasInterface(filter))
            {
                var grouptypes = from t in types
                                 group t by t.Name.Replace(filter.Split('*')[1], "").Replace(filter.Split('*')[0], "") into tGroup
                                 where tGroup.Count() == 2
                                 select tGroup.ToArray();

                foreach (var group in grouptypes)
                {
                    Type _interface = group.Where(x => x.IsInterface).First();
                    Type _implementation = group.Where(x => !x.IsInterface && _interface.IsAssignableFrom(x)).First();
                    // _services.AddSingleton(_interface, _implementation);
                    Console.WriteLine($"{_interface.Name}, {_implementation.Name}");
                    injector.DynamicInvoke(_interface, _implementation);
                }
            }
            else
                foreach (var implementation in types)
                {
                    Console.WriteLine($"{implementation.Name}");
                    if (!implementation.IsInterface) injector.DynamicInvoke(implementation);
                }
            /** @ENDImplementation */
        }
    }
}
