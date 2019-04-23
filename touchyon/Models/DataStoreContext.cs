using System;
using Microsoft.EntityFrameworkCore;

namespace touchyon.Models
{
    public class DataStoreContext: DbContext
    {
        public DataStoreContext(DbContextOptions<DataStoreContext> options) : base(options) { }

        public DbSet<Event> Events { set; get; }
    }
}
