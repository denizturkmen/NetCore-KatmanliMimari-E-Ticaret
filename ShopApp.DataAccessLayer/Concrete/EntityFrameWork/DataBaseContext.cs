using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ShopApp.Entities.Entity;

namespace ShopApp.DataAccessLayer.Concrete.EntityFrameWork
{
    public class DataBaseContext:DbContext
    {
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //burada hangi database provider kullanmak istiyorsak onu vercez (MsSQL,MySQL,Oracle...)
            optionsBuilder.UseSqlServer(@"Server=DENIZ-PC;Database=ShopDB;Integrated Security=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //FluentAPI iki alanıda PK olarak ayarlamak için
            modelBuilder.Entity<ProductCategory>()
                .HasKey(i => new {i.CategoryId, i.ProductId});
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        //Cart üzerindenn cartitemslere erişim var ve  cart item ile ilgili işlem yapmayacağım
        public DbSet<Cart> Carts { get; set; }

        //Sipariş işlemleri için
        public DbSet<Order> Orders { get; set; }


        }
}
