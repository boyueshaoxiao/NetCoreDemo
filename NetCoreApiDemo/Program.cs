using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using NLog;
using NLog.Web;

namespace NetCoreApiDemo
{
    /// <summary>
    /// 程序入口
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 程序主方法
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var logger = NLogBuilder
                .ConfigureNLog("XmlConfig/NLog.config")
                .GetCurrentClassLogger();
            try
            {
                logger.Debug("CreateHostBuilder Init.");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                logger.Debug($"CreateHostBuilder Running Fail！{e.Message}");
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        /// <summary>
        /// 构建主机
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //webBuilder.UseStartup<Startup>();
                    webBuilder.UseStartup(typeof(Startup).GetTypeInfo().Assembly.FullName ?? string.Empty);
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    //logging.AddConsole();
                })
                .UseNLog();
    }
}
