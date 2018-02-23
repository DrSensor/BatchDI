using System;
using BatchDI;

namespace BatchDI.AspNetCore
{
    using Microsoft.Extensions.DependencyInjection;

    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection BatchInject(this IServiceCollection services, Action<Type, Type> injector, dynamic filter, dynamic blacklist = null, bool parallel = false)
        {
            BatchDI.BatchInject(injector, filter, blacklist, parallel);
            return services;
        }

        public static IServiceCollection BatchInject(this IServiceCollection services, Action<Type> injector, dynamic filter, dynamic blacklist = null, bool parallel = false)
        {
            BatchDI.BatchInject(injector, filter, blacklist, parallel);
            return services;
        }

        public static IServiceCollection BatchSingleton(this IServiceCollection services, dynamic filter, dynamic blacklist = null, bool parallel = false)
        {

            if (BatchDI.filterHasInterface(filter))
            {
                Action<Type, Type> caller = (_interface, _implementation) => services.AddSingleton(_interface, _implementation);
                BatchDI.BatchInject(caller, filter, blacklist, parallel);
            }
            else
            {
                Action<Type> caller = (_implementation) => services.AddSingleton(_implementation);
                BatchDI.BatchInject(caller, filter, blacklist, parallel);
            }
            return services;
        }

        public static IServiceCollection BatchTransient(this IServiceCollection services, string filter, dynamic blacklist = null, bool parallel = false)
        {
            if (BatchDI.filterHasInterface(filter))
            {
                Action<Type, Type> caller = (_interface, _implementation) => services.AddTransient(_interface, _implementation);
                BatchDI.BatchInject(caller, filter, blacklist, parallel);
            }
            else
            {
                Action<Type> caller = (_implementation) => services.AddTransient(_implementation);
                BatchDI.BatchInject(caller, filter, blacklist, parallel);
            }
            return services;
        }

        public static IServiceCollection BatchScoped(this IServiceCollection services, string filter, dynamic blacklist = null, bool parallel = false)
        {
            if (BatchDI.filterHasInterface(filter))
            {
                Action<Type, Type> caller = (_interface, _implementation) => services.AddScoped(_interface, _implementation);
                BatchDI.BatchInject(caller, filter, blacklist, parallel);
            }
            else
            {
                Action<Type> caller = (_implementation) => services.AddScoped(_implementation);
                BatchDI.BatchInject(caller, filter, blacklist, parallel);
            }
            return services;
        }
    }
}
