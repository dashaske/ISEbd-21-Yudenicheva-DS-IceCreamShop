using System;
using System.Collections.Generic;
using System.Text;
using IceCreamShopBusinessLogic.ViewModel;

namespace IceCreamShopBusinessLogic.HelperModels
{
    public class ExcelInfoForWareHouse
    {
        public string FileName { get; set; }

        public string Title { get; set; }

        public List<ReportWareHouseIngredientsViewModel> WareHouseIngredients { get; set; }
    }
}
