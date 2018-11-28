using AzureNetCoreApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;

namespace AzureNetCoreApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                
            });

            services.AddSingleton<IConfiguration>(Configuration);

            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = WsFederationDefaults.AuthenticationScheme;
            })
                .AddWsFederation(WsFederationDefaults.AuthenticationScheme, options =>
                {
                    // MetadataAddress represents the Active Directory instance used to authenticate users.
                    options.MetadataAddress = Configuration["Azurekeys:AzureMetaDataUrl"];                    
                    options.Wtrealm = Configuration["Azurekeys:AzureWrealm"]; 
                    options.SaveTokens = true;
                    options.RequireHttpsMetadata = false;
                    

                }).AddCookie(
                    options =>
                    {

                        options.Cookie.Name = ".AspNet.SharedCookie";
                        options.Cookie.Expiration = TimeSpan.FromMinutes(20); 
                        options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                        options.Cookie.SameSite = SameSiteMode.None;
                        options.Cookie.HttpOnly = true;
                    });

            services.AddMvc();

           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            

            app.UseCookiePolicy();
            app.UseAuthentication();

            

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

