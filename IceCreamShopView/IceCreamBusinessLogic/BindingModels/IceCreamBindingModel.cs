using System;
using System.Collections.Generic;
using System.Text;

namespace IceCreamShopBusinessLogic.BindingModels
{
    public class IceCreamBindingModel
    {
        public int? Id { get; set; }
        public string IceCreamName { get; set; }
        public decimal Price { get; set; }
        public Dictionary<int, (string, int)> IceCreamIngredients { get; set; }
    }
}
