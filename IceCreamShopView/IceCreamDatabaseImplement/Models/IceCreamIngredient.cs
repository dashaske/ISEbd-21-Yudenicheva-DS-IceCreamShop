using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;

namespace IceCreamDatabaseImplement.Models
{
    public class IceCreamIngredient
    {
        public int Id { get; set; }

        public int IceCreamId { get; set; }

        public int IngredientId { get; set; }

        [Required]
        public int Count { get; set; }

        public virtual Ingredient Ingredient { get; set; }

        public virtual IceCream IceCream { get; set; }
    }
}
