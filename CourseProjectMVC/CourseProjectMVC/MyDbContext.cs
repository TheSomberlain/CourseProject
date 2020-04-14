using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseProjectMVC.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CourseProjectMVC
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options)
        {
                
        }

        public DbSet<Store> Stores { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<OrderProduct> OrderProduct { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Order> Orders { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Staff>()
                .HasOne<Store>(s => s.Store)
                .WithMany(st => st.Staff)
                .HasForeignKey(k => k.StoreId);

            builder.Entity<Stock>()
                .HasOne<Store>(s => s.Store)
                .WithOne(t => t.Stock)
                .HasForeignKey<Store>(z => z.StockId);

            builder.Entity<Order>()
                .HasOne<Customer>(ord => ord.Customer)
                .WithMany(cust => cust.Orders)
                .HasForeignKey(k => k.CustomerId);

            builder.Entity<Order>()
                .HasOne<Store>(ord => ord.Store)
                .WithMany(st => st.Orders)
                .HasForeignKey(k => k.StoreId);

            builder.Entity<OrderProduct>()
                .HasKey(op => new {op.OrderId, op.ProductId});
            builder.Entity<OrderProduct>()
                .HasOne(oc => oc.Order)
                .WithMany(k => k.OrderProducts)
                .HasForeignKey(key => key.OrderId);
            builder.Entity<OrderProduct>()
                .HasOne(oc => oc.Product)
                .WithMany(k => k.OrderProducts)
                .HasForeignKey(oc => oc.ProductId);

            builder.Entity<Stock>( b=>
                b.Property(s => s.Products)
                    .HasConversion(
                        d => JsonConvert.SerializeObject(d, Formatting.None),
                        s => JsonConvert.DeserializeObject<Dictionary<int, int>>(s)
                        )
                );
        }
    }
}
