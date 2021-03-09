using IceCreamShopBusinessLogic.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IceCreamDatabaseImplement.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int IceCreamId { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        public decimal Sum { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [Required]
        public DateTime DateCreate { get; set; }

        public DateTime? DateImplement { get; set; }
        public virtual IceCream IceCream { get; set; }
    }
}
