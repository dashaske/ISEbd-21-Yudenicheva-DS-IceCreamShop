using System;
using System.Collections.Generic;
using System.Text;
using IceCreamShopBusinessLogic.Enums;

namespace IceCreamShopBusinessLogic.ViewModel
{
    public class ReportOrdersViewModel
    {
        public DateTime DateCreate { get; set; }

        public string IceCreamName { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }

        public string Status { get; set; }
    }
}
