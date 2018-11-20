using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MPapi.Models
{
    public class MPapiContext : DbContext
    {
        public MPapiContext (DbContextOptions<MPapiContext> options)
            : base(options)
        {
        }

        public DbSet<MPapi.Models.ListingItem> ListingItem { get; set; }
    }
}
