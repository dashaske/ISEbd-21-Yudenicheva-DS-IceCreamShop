using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace IceCreamShopBusinessLogic.ViewModels
{
    public class WareHouseViewModel
    {
        public int Id { get; set; }

        [DisplayName("Название")]
        public string WareHouseName { get; set; }

        [DisplayName("ФИО ответственного")]
        public string ResponsiblePersonFCS { get; set; }

        [DisplayName("Дата создания")]
        public DateTime DateCreate { get; set; }
        public Dictionary<int, (string, int)> WareHouseIngredients { get; set; }
    }
}
