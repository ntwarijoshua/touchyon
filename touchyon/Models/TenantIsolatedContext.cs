using System;
using Microsoft.EntityFrameworkCore;
namespace touchyon.Models
{
    public class TenantIsolatedContext: DbContext
    {
        private readonly Event tenant;

        public TenantIsolatedContext(Event tenant)
        {
            this.tenant = tenant;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(tenant.ConnectionString);
            base.OnConfiguring(optionsBuilder);

        }
    }
}
