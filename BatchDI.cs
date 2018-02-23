using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AspNet.DependencyInjection.Batch
{
    public static partial class BatchDependencyInjectionExtensions
    {
        private static bool hasInterface(string filter)
        {
            return (!filter.StartsWith("*") && !filter.EndsWith("*")) ||
                    (filter.StartsWith("*") && filter.EndsWith("*"));
        }

        private static void inject(dynamic caller, dynamic filter, dynamic blacklist, bool parallel)
        {
            if (filter is string) BatchInjector(caller, filter, blacklist, parallel);
            else if (filter is string[]) foreach (var f in filter) BatchInjector(caller, f, blacklist, parallel);
            else throw new System.ArgumentException($"{nameof(filter)} must be `string` or `string[]`");
        }

        private static void BatchInjector(Delegate injector, string filter, dynamic blacklist, bool parallel)
        {
            // Filter based on namespace and provided pattern
            var types = from t in Assembly.GetEntryAssembly().GetTypes()
                        where containMainNamespace(t.Namespace) && filterMatch(t.Name) && notInBlacklist(t.Name)
                        select t;

            /** @Implementation */
            if (hasInterface(filter))
            {
                var grouptypes = from t in types
                                 group t by t.Name.Replace(filter.Split('*')[1], "").Replace(filter.Split('*')[0], "") into tGroup
                                 where tGroup.Count() == 2
                                 select tGroup;

                void invoke(IGrouping<string, Type> group)
                {
                    Type _interface = group.Where(x => x.IsInterface).First();
                    Type _implementation = group.Where(x => !x.IsInterface && _interface.IsAssignableFrom(x)).First();
                    // _services.AddSingleton(_interface, _implementation);
                    injector.DynamicInvoke(_interface, _implementation);
                }

                if (parallel) Parallel.ForEach(grouptypes, g => invoke(g));
                else foreach (var g in grouptypes) invoke(g);
            }
            else
            {
                void invoke(Type implementation) { if (!implementation.IsInterface) injector.DynamicInvoke(implementation); }
                if (parallel) Parallel.ForEach(types, t => invoke(t));
                else foreach (var t in types) invoke(t);
            }
            /** @ENDImplementation */

            /** @Helper */
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

            bool filterMatch(string className)
            {
                if (filter.StartsWith("*")) return className.EndsWith(filter.Replace("*", ""));
                else if (filter.EndsWith("*")) return className.StartsWith(filter.Replace("*", ""));
                else
                {
                    if (filter.Contains("*")) return className.Contains(filter.Split('*')[0]) || className.Contains(filter.Split('*')[1]);
                    else return className == filter;
                }
            }

            bool containMainNamespace(string _namespace)
            {
                try
                {
                    return _namespace.StartsWith(Assembly.GetEntryAssembly().EntryPoint.DeclaringType.Namespace ?? "");
                }
                catch (System.NullReferenceException)
                {
                    return false;
                }
            }
            /** @ENDHelper */
        }
    }
}
