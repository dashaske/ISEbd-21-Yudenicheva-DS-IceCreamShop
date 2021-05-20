using System;
using System.Collections.Generic;
using System.Text;

namespace IceCreamShopBusinessLogic.BindingModels
{
    public class WareHouseReplenishmentBindingModel
    {
        public int IngredientId { get; set; }
        public int WareHouseId { get; set; }
        public int Count { get; set; }
    }
}
