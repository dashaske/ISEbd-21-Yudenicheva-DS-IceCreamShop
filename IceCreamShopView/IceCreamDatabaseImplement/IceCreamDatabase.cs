using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using IceCreamDatabaseImplement.Models;
using System.Text;

namespace IceCreamDatabaseImplement
{
    public class IceCreamDatabase : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog= IceCreamDatabase;Integrated Security=True;MultipleActiveResultSets=True;");
            }
            base.OnConfiguring(optionsBuilder);
        }
        public virtual DbSet<Ingredient> Ingredients { set; get; }

        public virtual DbSet<IceCream> IceCreams { set; get; }

        public virtual DbSet<IceCreamIngredient> IceCreamIngredients { set; get; }

        public virtual DbSet<Order> Orders { set; get; }

        public virtual DbSet<WareHouse> WareHouses { set; get; }

        public virtual DbSet<WareHouseIngredient> WareHouseIngredients { set; get; }
    }
}
