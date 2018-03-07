using Microsoft.AspNetCore.Hosting;
using System.Reflection;
using BatchDI;

namespace BatchDI.AspNetCore
{
    public static class IWebHostBuilderExtensions
    {
        public static IWebHostBuilder SetBatchDIEntryPoint<T>(this IWebHostBuilder webhost)
        {
            BatchDI.SetEntryAssembly(Assembly.GetAssembly(typeof(T)));
            return webhost;
        }
    }
}