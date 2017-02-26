using System;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OnionArchitectures.Repository;
using OnionArchitectures.Service;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OnionArchitectures.Common;

namespace OnionArchitectures
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
           
                services.AddMvc();
                services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
                services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                services.AddTransient<IUserService, UserService>();
                services.AddTransient<IUserProfileService, UserProfileService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            try
            {

                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();
                app.UseStaticFiles();

                // Add MVC to the request pipeline.
                app.UseCors(builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());


                app.UseExceptionHandler(
                  builder =>
                  {
                      builder.Run(
                        async context =>
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            context.Response.ContentType = "application/json";
                            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                            var error = context.Features.Get<IExceptionHandlerFeature>();
                            if (error != null)
                            {
                                var err = JsonConvert.SerializeObject(new Error()
                                {
                                    Stacktrace = error.Error.StackTrace,
                                    Message = error.Error.Message
                                });
                                await context.Response.WriteAsync(err).ConfigureAwait(false);
                            }
                        });
                  });

                app.UseMvc(routes =>
                {
                    routes.MapRoute(
                        "default",
                        "{controller=Home}/{action=Index}/{id?}");


                });

            }
            catch (Exception ex)
            {
                app.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var err = JsonConvert.SerializeObject(new Error()
                    {
                        Stacktrace = ex.StackTrace,
                        Message = ex.Message
                    });
                    await context.Response.WriteAsync(err).ConfigureAwait(false);

                });
            }
        }
    }
}
