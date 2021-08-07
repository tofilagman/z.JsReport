using jsreport.Local;
using jsreport.Shared;
using jsreport.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.IO;

namespace z.JsReport
{
    public static class ReportServiceInjector
    {
        public static void AddReportServiceInjector(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            //var cwd = Path.Combine(environment.ContentRootPath, "JSReport");

            //if (!Directory.Exists(cwd))
            //    Directory.CreateDirectory(cwd);

            var cwd = "/var/tmp/jsreport";

            var lr = new LocalReporting()
               .KillRunningJsReportProcesses()
#if IsWindows
               .UseBinary(jsreport.Binary.JsReportBinary.GetBinary())
#endif
#if IsLinux
               .UseBinary(jsreport.Binary.Linux.JsReportBinary.GetBinary())
#endif
#if IsOSX
               .UseBinary(jsreport.Binary.OSX.JsReportBinary.GetBinary()) 
#endif
               .Configure(cfg =>
                new Configuration()
                {
                    Chrome = new ChromeConfiguration() { Timeout = 1000 * 60 * 10 },
                    Extensions = new ExtensionsConfiguration()
                    {
                        Express = new ExpressConfiguration() { InputRequestLimit = "100mb" },
                        Scripts = new ScriptsConfiguration() { }
                    },
                    // explicitly set port, because azure web app sets environment variable PORT
                    // which is used also by jsreport
                    HttpPort = 1000,
                    AllowLocalFilesAccess = true,
                    AppPath = cwd,
                    TempDirectory = Path.Combine(cwd, "temp"),
                    Discover = false
                })
               .AsUtility()
               .KeepAlive(!Debugger.IsAttached)
               .Create();

            //jasper report instance
            services.AddSingleton<IRenderService>(lr);
        }
    }
}
