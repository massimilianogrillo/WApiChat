using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lavoro.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using WebApi.Hubs;
using WebApi.Services;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// To be used to log queries
        /// </summary>
        public readonly LoggerFactory MyConsoleLoggerFactory =
            new LoggerFactory(new[] {
                new ConsoleLoggerProvider((category, level)
                    => category== DbLoggerCategory.Database.Command.Name
                    && level== LogLevel.Information, true
                )
            });

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddSignalR();


            services.AddCors(options => {
                options.AddPolicy("collaborativo", 
                    policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());
            });

            //lazy loading is used to allow access of the refrenced data to another table using foreign keys
            services.AddDbContext<LaboroContext>(
                options => {
                    options.UseSqlServer(Configuration.GetConnectionString("DbConnection"));
                    options.UseLoggerFactory(MyConsoleLoggerFactory);
                    options.UseLazyLoadingProxies();
                } );

            // add services for dependency injection
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFileProviderService, FileProviderService>();

            services.AddMvc();//.SetCompatibilityVersion(CompatibilityVersion.);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("collaborativo");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // signalR will listen to <server ulr>/chat
            app.UseSignalR(routes=> {
                routes.MapHub<MessageHub>("/chat");
            });
            
            app.UseMvc();
        }
    }
}
