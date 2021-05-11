using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using VideoManager.Infrastructure;

namespace VideoManager.GUI
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                using IHost host = CreateHostBuilder(args)
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddSingleton<MainForm>();
                        services.AddDomainLayer();
                        services.AddInfrastructureLayer(hostContext.Configuration);
                        services.AddAutoMapper(typeof(InfrastructureMappingProfile), typeof(DomainMappingProfile));
                    })
                    .Build();

                using (IServiceScope serviceScope = host.Services.CreateScope())
                {
                    Application.SetHighDpiMode(HighDpiMode.SystemAware);
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    IServiceProvider services = serviceScope.ServiceProvider;
                    Form mainForm = services.GetRequiredService<MainForm>();

                    Application.Run(mainForm);
                }
            }
            catch (Exception ex)
            {
                // Log.Logger will likely be internal type "Serilog.Core.Pipeline.SilentLogger".
                if (Log.Logger == null || Log.Logger.GetType().Name == "SilentLogger")
                {
                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .WriteTo.Debug()
                        .CreateLogger();
                }

                Log.Fatal(ex, "¯\\(°_o)/¯ Something went wrong and the application just crashed!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host
            .CreateDefaultBuilder(args)
                    .ConfigureServices(s => s
                    .AddLogging()
                    .BuildServiceProvider(true))
            .ConfigureAppConfiguration(c => c
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.local.json", true)
                .AddEnvironmentVariables()
                .AddCommandLine(args))
            .UseSerilog((hostingContext, loggerConfiguration) => {
                loggerConfiguration
                    .ReadFrom.Configuration(hostingContext.Configuration)
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
                    .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment);

#if DEBUG
                // Used to filter out potentially bad data due debugging.
                // Very useful when doing Seq dashboards and want to remove logs under debugging session.
                loggerConfiguration.Enrich.WithProperty("DebuggerAttached", Debugger.IsAttached);
#endif
            });
        }
    }
}
