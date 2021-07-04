using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace webapi
{
    public class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        private static IHostEnvironment Env { get; set; }
        public static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                    .ConfigureAppConfiguration((context, config) =>
                    {
                        Env = context.HostingEnvironment;
                        config.AddEnvironmentVariables()
                              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                              .AddJsonFile($"appsettings.{Env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                              .AddCommandLine(args);
                        Configuration = config.Build();
                    })
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        //webBuilder.UseStartup<Startup>();
                        webBuilder.UseUrls("http://localhost:4000");
                        webBuilder.Configure(app =>
                        {
                            if (Env.IsDevelopment())
                            {
                                app.UseDeveloperExceptionPage();
                                app.UseSwagger();
                                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "template v1"));
                            }
                            app.UseRouting();
                            app.UseEndpoints(endpoints =>
                            {
                                endpoints.MapControllers();
                            });
                        });
                    })
                    .ConfigureServices(services =>
                    {
                        //services.AddSingleton<IHostedService, HostedService>();
                        services.AddControllers();
                        services.AddSwaggerGen(c =>
                        {
                            c.SwaggerDoc("v1", new OpenApiInfo { Title = "template", Version = "v1" });
                        });
                    })
                    .Build()
                    .RunAsync();
        }
    }
}
