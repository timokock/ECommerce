using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class ECommerceDbContext : DbContext
    {
        // Constructor : base para heredar el constructor de la super clase
        public ECommerceDbContext() : base("DefaultConnection")
        {
        }

        public DbSet<Department> Departments { get; set; }

        public System.Data.Entity.DbSet<ECommerce.Models.City> Cities { get; set; }
    }
}