using System;
using System.Collections.Generic;
using System.Text;

namespace IceCreamShopBusinessLogic.ViewModel
{
    public class ReportWareHouseIngredientsViewModel
    {
        public string WareHouseName { get; set; }

        public int TotalCount { get; set; }

        public List<Tuple<string, int>> Ingredients { get; set; }
    }
}
