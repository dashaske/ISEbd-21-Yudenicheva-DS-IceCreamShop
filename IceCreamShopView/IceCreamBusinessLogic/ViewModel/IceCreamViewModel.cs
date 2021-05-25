using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using IceCreamShopBusinessLogic.Attributes;

namespace IceCreamShopBusinessLogic.ViewModels
{
    [DataContract]
    public class IceCreamViewModel
    {
        [DataMember]
        [Column(title: "Номер", width: 100, visible: false)]
        public int Id { get; set; }
        [DataMember]
        [Column(title: "Название мороженого", gridViewAutoSize: GridViewAutoSize.Fill)]
        public string IceCreamName { get; set; }
        [DataMember]
        [Column(title: "Цена", width: 100)]
        public decimal Price { get; set; }
        [DataMember]
        public Dictionary<int, (string, int)> IceCreamIngredients { get; set; }

    }
}
