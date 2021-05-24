using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;

namespace IceCreamDatabaseImplement.Models
{
    public class Ingredient
    {
        public int Id { get; set; }

        [Required]
        public string IngredientName { get; set; }

        [ForeignKey("IngredientId")]
        public virtual List<IceCreamIngredient> IceCreamIngredients { get; set; }

        [ForeignKey("IngredientId")]
        public virtual List<WareHouseIngredient> WareHouseIngredient { get; set; }
    }
}
