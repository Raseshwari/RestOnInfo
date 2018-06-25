using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RestOnInfo.Data;
using RestOnInfo.Services;

namespace RestOnInfo
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        //Register your custom made services to the container.
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddOpenIdConnect(options => 
            {
                _configuration.Bind("AzureAd", options);
            })
            .AddCookie();

            services.AddSingleton<IGreeter, Greeter>();
            services.AddDbContext<RestOnInfoDbContext>(
                options => options.UseSqlServer(_configuration.GetConnectionString("RestOnInfo")));
            services.AddScoped<IRestaurantData, SqlRestaurantData>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IGreeter greeter, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRewriter(new RewriteOptions().AddRedirectToHttpsPermanent());

            app.UseWelcomePage(new WelcomePageOptions {
                Path = "/wp"
            });

            //app.UseDefaultFiles();
            app.UseStaticFiles();
            //app.UseFileServer();

            //middleware for authentication
            app.UseAuthentication();

            app.UseMvc(ConfigureRoutes);

            app.Run(async (context) =>
            {
                var greeting = greeter.GetMessageOfTheDay();
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync($"Not found");
            });
        }

        private void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute("Default","{controller=Home}/{action=Index}/{id?}");
            //routeBuilder.MapRoute("About", "{controller}/{action}/{id?}");
        }
    }
}
