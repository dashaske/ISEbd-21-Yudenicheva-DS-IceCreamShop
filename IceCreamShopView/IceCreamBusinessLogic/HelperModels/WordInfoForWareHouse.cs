using System;
using System.Collections.Generic;
using System.Text;
using IceCreamShopBusinessLogic.ViewModels;

namespace IceCreamShopBusinessLogic.HelperModels
{
    public class WordInfoForWareHouse
    {
        public string FileName { get; set; }

        public string Title { get; set; }

        public List<WareHouseViewModel> WareHouses { get; set; }
    }
}
