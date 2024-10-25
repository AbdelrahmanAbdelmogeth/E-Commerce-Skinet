﻿using ECommerceSkinet.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ECommerceSkinet.Infrastructure.DatabaseContext
{
    public class ApplicationDbContext : DbContext { 
        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set;}
        public DbSet<ProductType> ProductTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
