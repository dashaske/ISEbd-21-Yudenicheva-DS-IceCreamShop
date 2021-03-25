using System;
using System.Collections.Generic;
using IceCreamShopBusinessLogic.ViewModel;
using System.Text;

namespace IceCreamShopBusinessLogic.HelperModels
{
    class ExcelInfo
    {
        public string FileName { get; set; }

        public string Title { get; set; }

        public List<ReportIngredientIceCreamViewModel> IngredientIceCreams { get; set; }
    }
}
