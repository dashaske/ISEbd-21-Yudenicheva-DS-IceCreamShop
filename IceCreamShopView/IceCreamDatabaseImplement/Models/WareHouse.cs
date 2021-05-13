using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IceCreamDatabaseImplement.Models
{
    public class WareHouse
    {
        public int Id { get; set; }

        [Required]
        public string WareHouseName { get; set; }

        [Required]
        public string ResponsiblePersonFCS { get; set; }

        [Required]
        public DateTime DateCreate { get; set; }

        [ForeignKey("WareHouseId")]
        public virtual List<WareHouseIngredient> WareHouseIngredients { get; set; }
    }
}
