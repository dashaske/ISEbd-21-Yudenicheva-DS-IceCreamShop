using System;
using System.Collections.Generic;
using System.Text;

namespace IceCreamShopBusinessLogic.BindingModel
{ //смена статуса
    public class ChangeStatusBindingModel
    {
        public int OrderId { get; set; }
        public int IceCreamId { get; set; }
        public int Count { get; set; }
        public decimal Sum { get; set; }
    }
}
