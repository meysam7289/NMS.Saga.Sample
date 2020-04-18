using System;
using GreenPipes;
using MassTransit;
using MassTransit.Courier.Factories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NMS.Saga.Sample.Contracts;
using NMS.Saga.Sample.Contracts.Models;
using NMS.Saga.Sample.Kachar.Courier;
using NMS.Saga.Sample.Kachar.DataLayer;

namespace NMS.Saga.Sample.Kachar
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
            services.AddDbContextPool<KacharDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetValue<string>("ConnectionString"),
                    serverDbContextOptionsBuilder =>
                    {
                        var minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
                        serverDbContextOptionsBuilder.CommandTimeout(minutes);
                        serverDbContextOptionsBuilder.EnableRetryOnFailure();
                    }
            );
            });

            services.AddScoped<InsertCodingActivity>();

            services.AddMassTransit(x =>
            {
                x.AddBus(provider => BusConfigurator.ConfigureBus((cfg) =>
                {
                    cfg.ReceiveEndpoint("Kachar_Coding_Insert", e =>
                    {
                        e.PrefetchCount = 100;
                        e.ExecuteActivityHost<InsertCodingActivity, Coding>(new Uri(RabbitMqConstants.RabbitMqUri + "Comp_Kachar_Coding_Insert"), services.BuildServiceProvider(),
                              c => c.UseRetry(r => r.Immediate(5)));
                    });
                    cfg.ReceiveEndpoint("Comp_Kachar_Coding_Insert", e =>
                    {
                        e.PrefetchCount = 100;
                        e.CompensateActivityHost<InsertCodingActivity, Coding>(services.BuildServiceProvider(), c => c.UseRetry(r => r.Immediate(5)));
                    });
                }));
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseMassTransit();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
