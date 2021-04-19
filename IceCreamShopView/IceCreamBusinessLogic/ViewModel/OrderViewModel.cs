using IceCreamShopBusinessLogic.Enums;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace IceCreamShopBusinessLogic.ViewModels
{
    [DataContract]
    public class OrderViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int ClientId { get; set; }

        [DataMember]
        public int IceCreamId { get; set; }

        [DataMember]
        public int? ImplementerId { get; set; }

        [DataMember]
        [DisplayName("Клиент")]
        public string ClientFIO { get; set; }

        [DataMember]
        [DisplayName("Исполнитель")]
        public string ImplementerFIO { get; set; }

        [DataMember]
        [DisplayName("Мороженое")]
        public string IceCreamName { get; set; }

        [DataMember]
        [DisplayName("Количество")]
        public int Count { get; set; }

        [DataMember]
        [DisplayName("Сумма")]
        public decimal Sum { get; set; }

        [DataMember]
        [DisplayName("Статус")]
        public OrderStatus Status { get; set; }

        [DataMember]
        [DisplayName("Дата создания")]
        public DateTime DateCreate { get; set; }


        [DisplayName("Дата выполнения")]
        public DateTime? DateImplement { get; set; }
    }
}
