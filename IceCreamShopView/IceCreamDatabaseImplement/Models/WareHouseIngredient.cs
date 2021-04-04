using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IceCreamDatabaseImplement.Models
{
    public class WareHouseIngredient
    {
        public int Id { get; set; }

        public int WareHouseId { get; set; }

        public int IngredientId { get; set; }

        [Required]
        public int Count { get; set; }

        public virtual Ingredient Ingredient { get; set; }

        public virtual WareHouse WareHouse { get; set; }
    }
}
