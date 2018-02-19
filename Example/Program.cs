using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Example
{
    public class Program
    {
        public static int Main(string[] args)
        {
            int timeout = 0;
            bool timoutdefined = args.Length != 0 ? int.TryParse(args[0], out timeout) : false;
            if (timoutdefined) args = args.Skip(1).ToArray();

            var task = Task.Factory.StartNew(() => BuildWebHost(args).Run());
            while (!timoutdefined) { }

            task.Wait(timeout * 1000);
            return 0;
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
