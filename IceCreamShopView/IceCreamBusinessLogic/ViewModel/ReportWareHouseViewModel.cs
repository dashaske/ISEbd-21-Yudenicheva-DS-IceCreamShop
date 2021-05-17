using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace IceCreamShopBusinessLogic.ViewModel
{
    public class ReportWareHouseViewModel
    {
        public int Id { get; set; }

        [DisplayName("Название мороженого")]
        public string IceCreamName { get; set; }

        [DisplayName("Цена")]
        public decimal Price { get; set; }

        public Dictionary<int, (string, int)> IngredientIceCreams { get; set; }
    }
}
