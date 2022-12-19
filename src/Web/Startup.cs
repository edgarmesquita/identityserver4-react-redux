using System;
using IdentityServer.Application.Extensions;
using IdentityServer.Application.Models;
using IdentityServer.Application.Models.Data;
using IdentityServer.Application.Models.Settings;
using IdentityServer.Persistence;
using IdentityServer.Persistence.Extensions;
using IdentityServer4;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer.Web;

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
        var settings = Configuration.Get<AppSettings>();
        services.AddControllersWithViews();
        
        services.AddDbContext<MainContext>(opt =>
            opt.UseMainPersistence(Configuration, settings.Database));
        services.AddDbContext<ConfigurationContext>(opt =>
            opt.UseMainPersistence(Configuration, settings.Database));
        
        services.AddIdentity<UserData, RoleData>()
            .AddEntityFrameworkStores<MainContext>()
            .AddDefaultTokenProviders();
        
        var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;
            })
            .AddConfigurationStore<ConfigurationContext>(options => options.ResolveDbContextOptions = (provider, optionsBuilder) => 
                optionsBuilder.UseMainPersistence(Configuration, settings.Database))
            .AddOperationalStore(options => options.ResolveDbContextOptions = (provider, optionsBuilder) => 
                optionsBuilder.UseMainPersistence(Configuration, settings.Database))
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients)
            .AddAspNetIdentity<UserData>();
        
        // not recommended for production - you need to store your key material somewhere secure
        builder.AddDeveloperSigningCredential();
        
        services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    
                // register your IdentityServer with Google at https://console.developers.google.com
                // enable the Google+ API
                // set the redirect URI to https://localhost:5001/signin-google
                options.ClientId = "copy client ID from Google here";
                options.ClientSecret = "copy client secret from Google here";
            })
            .AddFacebook(options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                options.AppId = "99999999999999999";
                options.AppSecret = "copy app secret from Facebook here";
            });
        
        // In production, the React files will be served from this directory
        services.AddSpaStaticFiles(configuration =>
        {
            configuration.RootPath = "ClientApp/build";
        });

        services.AddApplication();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseSpaStaticFiles();

        app.UseRouting();

        app.UseIdentityServer();
        app.UseAuthorization();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");
        });

        app.UseSpa(spa =>
        {
            spa.Options.SourcePath = "ClientApp";
            
            if (env.IsDevelopment())
            {
                //spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
                spa.UseReactDevelopmentServer(npmScript: "start");
            }
        });
    }
}