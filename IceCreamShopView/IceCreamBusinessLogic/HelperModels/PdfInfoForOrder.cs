using System;
using System.Collections.Generic;
using System.Text;
using IceCreamShopBusinessLogic.ViewModel;

namespace IceCreamShopBusinessLogic.HelperModels
{
    public class PdfInfoForOrder
    {
        public string FileName { get; set; }

        public string Title { get; set; }

        public List<ReportOrderByDateViewModel> Orders { get; set; }
    }
}
