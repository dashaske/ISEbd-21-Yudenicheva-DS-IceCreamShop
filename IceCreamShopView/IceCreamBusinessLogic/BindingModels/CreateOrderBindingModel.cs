using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace IceCreamShopBusinessLogic.BindingModel
{
    [DataContract]
    public class CreateOrderBindingModel
    { //создание заказа
        [DataMember]
        public int ClientId { get; set; }

        [DataMember]
        public int IceCreamId { get; set; }

        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public decimal Sum { get; set; }
    }
}
