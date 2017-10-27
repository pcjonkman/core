using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Core.Models;
using Core.Models.Identity;
using Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Core.Authorization;

namespace Core
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // https://stackoverflow.com/questions/44180773/dependency-injection-in-asp-net-core-2-thows-exception
            // services.AddDbContext<CoreContext>(opt => opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddDbContext<CoreContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddMvc();


            // Authorization handlers.
            services.AddScoped<IAuthorizationHandler, ContactIsOwnerAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, ContactAdministratorsAuthorizationHandler>();
            // services.AddSingleton<IAuthorizationHandler, ContactManagerAuthorizationHandler>();

            // Build the intermediate service provider then return it
            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider sp)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions {
                    HotModuleReplacement = true,
                    // EventSource's response has a MIME type ("text/html") that is not "text/event-stream".
                    // https://github.com/aspnet/JavaScriptServices/issues/1204
                    HotModuleReplacementEndpoint = "/dist/__webpack_hmr"
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });

            // Set password with the Secret Manager tool.
            // dotnet user-secrets set SeedUserPW <pw>
            //  Windows: %APPDATA%\microsoft\UserSecrets\<userSecretsId>\secrets.json
            //  Linux: ~/.microsoft/usersecrets/<userSecretsId>/secrets.json
            //  Mac: ~/.microsoft/usersecrets/<userSecretsId>/secrets.json
            var userPassword = Configuration["SeedUserPW"];

            if (String.IsNullOrEmpty(userPassword))
            {
                throw new System.Exception("Use secrets manager to set SeedUserPW \n" +
                                           "dotnet user-secrets set SeedUserPW <pw>");
            }


            try
            {
                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    serviceScope.ServiceProvider.GetService<ApplicationDbContext>().Database.Migrate();
                    serviceScope.ServiceProvider.GetService<CoreContext>().Database.Migrate();

                    // disable next line for creating (first and new) migration
                    app.SeedData(env.IsDevelopment(), userPassword);


                    // var userManager = app.ApplicationServices.GetService<UserManager<ApplicationUser>>();
                    // var roleManager = app.ApplicationServices.GetService<RoleManager<IdentityRole>>();

                    // serviceScope.ServiceProvider.GetService<CoreContext>().EnsureSeedData(userManager, roleManager);
                }
            }
            catch {
                throw new System.Exception("You need to update the DB "
                    + "\nPM > Update-Database " + "\n or \n" +
                    "> dotnet ef database update"
                    + "\nIf that doesn't work, comment out SeedData and "
                    + "register a new user");
            }
        }
    }
}
