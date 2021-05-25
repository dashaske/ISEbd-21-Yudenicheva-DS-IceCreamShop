using System;
using IceCreamShopListImplement.Models;
using System.Collections.Generic;
using System.Text;

namespace IceCreamShopListImplement
{
    public class DataListSingleton
    {
        private static DataListSingleton instance;
        public List<Ingredient> Ingredients { get; set; }
        public List<Order> Orders { get; set; }
        public List<IceCream> IceCreams { get; set; }
        public List<WareHouse> Warehouses { get; set; }
        public List<Client> Clients { get; set; }
        private DataListSingleton()
        {
            Ingredients = new List<Ingredient>();
            Orders = new List<Order>();
            IceCreams = new List<IceCream>();
            Warehouses = new List<WareHouse>();
            Clients = new List<Client>();
        }
        public static DataListSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new DataListSingleton();
            }
            return instance;
        }
    }
}
