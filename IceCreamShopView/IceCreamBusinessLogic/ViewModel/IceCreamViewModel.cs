using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace IceCreamShopBusinessLogic.ViewModels
{
    [DataContract]
    public class IceCreamViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        [DisplayName("Название мороженого")]
        public string IceCreamName { get; set; }
        [DataMember]
        [DisplayName("Цена")]
        public decimal Price { get; set; }
        [DataMember]
        public Dictionary<int, (string, int)> IceCreamIngredients { get; set; }

    }
}
