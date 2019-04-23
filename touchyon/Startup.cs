using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using touchyon.Models;
using touchyon.Core;
using System.Collections.ObjectModel;

namespace touchyon
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
            services.AddDbContext<DataStoreContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.Configure<MultiTenancyOptions>(opts =>
            {
                var scopeFactory = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();
                using (var scope = scopeFactory.CreateScope())
                {
                    var provider = scope.ServiceProvider;
                    using (var masterContext = provider.GetRequiredService<DataStoreContext>())
                    {
                        opts.Tenants = masterContext.Events.ToList();
                    }
                }
            });
            services.AddMultitenancy<Event, TenantResolver>();
            services.AddEntityFrameworkNpgsql().AddDbContext<TenantIsolatedContext>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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
                app.UseHsts();
            }
            app.UseMultitenancy<Event>();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
