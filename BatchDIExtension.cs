using System;

namespace AspNet.DependencyInjection.Batch
{
    using Microsoft.Extensions.DependencyInjection;

    public static partial class BatchDependencyInjectionExtensions
    {
        public static IServiceCollection BatchInject(this IServiceCollection services, Action<Type, Type> injector, dynamic filter, dynamic blacklist = null, bool parallel = false)
        {
            inject(injector, filter, blacklist, parallel);
            return services;
        }

        public static IServiceCollection BatchInject(this IServiceCollection services, Action<Type> injector, dynamic filter, dynamic blacklist = null, bool parallel = false)
        {
            inject(injector, filter, blacklist, parallel);
            return services;
        }

        public static IServiceCollection BatchSingleton(this IServiceCollection services, dynamic filter, dynamic blacklist = null, bool parallel = false)
        {

            if (hasInterface(filter))
            {
                Action<Type, Type> caller = (_interface, _implementation) => services.AddSingleton(_interface, _implementation);
                inject(caller, filter, blacklist, parallel);
            }
            else
            {
                Action<Type> caller = (_implementation) => services.AddSingleton(_implementation);
                inject(caller, filter, blacklist, parallel);
            }
            return services;
        }

        public static IServiceCollection BatchTransient(this IServiceCollection services, string filter, dynamic blacklist = null, bool parallel = false)
        {
            if (hasInterface(filter))
            {
                Action<Type, Type> caller = (_interface, _implementation) => services.AddTransient(_interface, _implementation);
                inject(caller, filter, blacklist, parallel);
            }
            else
            {
                Action<Type> caller = (_implementation) => services.AddTransient(_implementation);
                inject(caller, filter, blacklist, parallel);
            }
            return services;
        }

        public static IServiceCollection BatchScoped(this IServiceCollection services, string filter, dynamic blacklist = null, bool parallel = false)
        {
            if (hasInterface(filter))
            {
                Action<Type, Type> caller = (_interface, _implementation) => services.AddScoped(_interface, _implementation);
                inject(caller, filter, blacklist, parallel);
            }
            else
            {
                Action<Type> caller = (_implementation) => services.AddScoped(_implementation);
                inject(caller, filter, blacklist, parallel);
            }
            return services;
        }
    }
}
