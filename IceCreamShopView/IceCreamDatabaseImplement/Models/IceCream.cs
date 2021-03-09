using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;

namespace IceCreamDatabaseImplement.Models
{
    public class IceCream
    {
        public int Id { get; set; }

        [ForeignKey("IceCreamId")]
        public virtual List<IceCreamIngredient> IceCreamIngredients { get; set; }

        [ForeignKey("IceCreamId")]
        public virtual List<Order> Orders { get; set; }

        [Required]
        public string IceCreamName { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
