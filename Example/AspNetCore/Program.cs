using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using McMaster.Extensions.CommandLineUtils;

namespace Example
{
    using Services;

    public class Program
    {
        public static int Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", optional: true)
                .AddCommandLine(args)
                .Build();

            var app = new CommandLineApplication();
            app.HelpOption();
            var optionSubject = app.Option("-s|--shutdown <seconds>", "The subject", CommandOptionType.SingleValue);

            app.OnExecute(() =>
            {
                int timeout = optionSubject.HasValue() ? int.Parse(optionSubject.Value()) : 0;

                if (timeout > 0) Console.WriteLine($"Shutting down in {timeout}s!");
                var task = Task.Factory.StartNew(() => BuildWebHost(args, config).Run());
                while (timeout <= 0) { }

                task.Wait(timeout * 1000);
                return 0;
            });

            return app.Execute(args);

        }

        public static IWebHost BuildWebHost(string[] args, IConfiguration config) =>
            WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(config)
                .UseStartup<Startup>()
                .Build();
    }
}
