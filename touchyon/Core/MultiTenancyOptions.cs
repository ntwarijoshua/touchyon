using System;
using System.Collections.Generic;
using touchyon.Models;
namespace touchyon.Core
{
    public class MultiTenancyOptions
    {
        public ICollection<Event> Tenants { get; set; } = new List<Event>();
    }
}
