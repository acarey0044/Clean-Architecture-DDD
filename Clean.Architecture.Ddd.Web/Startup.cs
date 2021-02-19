using Autofac;
using Clean.Architecture.Ddd.Application;
using Clean.Architecture.Ddd.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Clean.Architecture.Ddd.Web
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
            services.Configure<CookiePolicyOptions>(opts =>
            {
                opts.MinimumSameSitePolicy = SameSiteMode.Strict;
            });
            services.ConfigureApplicationCookie(opts =>
            {
                opts.Cookie.HttpOnly = true;
                opts.ExpireTimeSpan = TimeSpan.FromHours(1);
                opts.LoginPath = "/Account/Login";
                opts.LogoutPath = "/Account/Logout";
                opts.Cookie = new CookieBuilder
                {
                    IsEssential = true
                };
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opts =>
                {
                    opts.Cookie.HttpOnly = true;
                    opts.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    opts.Cookie.SameSite = SameSiteMode.Lax;
                });
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();
            services.AddControllers();
        }

        public void AddDbContexts(IServiceCollection services)
        {
            services.AddDbContext<IdentityContext>(opts => opts.UseInMemoryDatabase("Identity"));
        }

        public void AddMediatr(ContainerBuilder builder)
        {
            builder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();

            builder.Register<ServiceFactory>(ctx =>
            {
                var component = ctx.Resolve<IComponentContext>();
                return type => component.Resolve(type);
            });

            //TODO: Add pipeline behaviors
            builder.RegisterAssemblyTypes(typeof(TestMediatrCommand).Assembly).AsImplementedInterfaces();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            AddMediatr(builder);
        }
    }
}
