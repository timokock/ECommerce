﻿using System;
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

        public System.Data.Entity.DbSet<ECommerce.Models.Department> Departments { get; set; }
    }
}