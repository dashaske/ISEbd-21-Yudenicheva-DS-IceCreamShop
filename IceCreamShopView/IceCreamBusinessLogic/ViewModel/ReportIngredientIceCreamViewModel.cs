using System;
using System.Collections.Generic;
using System.Text;

namespace IceCreamShopBusinessLogic.ViewModel
{
    public class ReportIngredientIceCreamViewModel
    {
        public string IceCreamName { get; set; }

        public int TotalCount { get; set; }

        public List<Tuple<string, int>> Ingredients { get; set; } 
    }
}
