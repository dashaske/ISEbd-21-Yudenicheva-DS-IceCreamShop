using System;
using System.Collections.Generic;
using System.ComponentModel;
using IceCreamShopBusinessLogic.Attributes;

namespace IceCreamShopBusinessLogic.ViewModels
{
    public class IngredientViewModel
    {
        [Column(title: "Номер", width: 100, visible: false)]
        public int Id { get; set; }
        [Column(title: "Название ингредиента", gridViewAutoSize: GridViewAutoSize.Fill)]
        public string IngredientName { get; set; }
    }
}
