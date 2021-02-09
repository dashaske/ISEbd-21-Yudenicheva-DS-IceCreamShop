using System;
using System.Collections.Generic;
using System.Text;

namespace IceCreamShopBusinessLogic.BindingModel
{
    public class CreateOrderBindingModel
    { //создание заказа
        public int IceCreamId { get; set; }
        public int Count { get; set; }
        public decimal Sum { get; set; }
    }
}