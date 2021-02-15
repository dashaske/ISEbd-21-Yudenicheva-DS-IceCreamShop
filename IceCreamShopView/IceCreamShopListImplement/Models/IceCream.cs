using System;
using System.Collections.Generic;
using System.Text;

namespace IceCreamShopListImplement.Models
{
    public class IceCream
    {
        public int Id { get; set; }
        public string IceCreamName { get; set; }
        public decimal Price { get; set; }
        public Dictionary<int, int> IceCreamIngredients { get; set; }
    }
}
