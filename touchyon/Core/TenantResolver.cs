using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SaasKit.Multitenancy;
using touchyon.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace touchyon.Core
{
    public class TenantResolver: MemoryCacheTenantResolver<Event>
    { 
        private readonly ICollection<Event> _tenants;
        public TenantResolver(IOptions<MultiTenancyOptions> options,IMemoryCache memoryCache, ILoggerFactory loggerFactory) : base(memoryCache, loggerFactory)
        {
            _tenants = options.Value.Tenants;
        }


        protected override string GetContextIdentifier(HttpContext context)
        {
            return context.Request.Host.Host.ToLower();
        }

        protected override IEnumerable<string> GetTenantIdentifiers(TenantContext<Event> context)
        {
            return new [] { context.Tenant.HostName};
        }

        protected override Task<TenantContext<Event>> ResolveAsync(HttpContext context)
        {
            TenantContext<Event> tenantContext = null;
            var tenant = _tenants.FirstOrDefault(t => t.HostName.Equals(context.Request.Host.Host.ToLower()));
            if (tenant != null)
            {
                tenantContext = new TenantContext<Event>(tenant);
            }

            return Task.FromResult(tenantContext);
        }
    }
}
