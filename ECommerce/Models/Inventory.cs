using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models
{
    public class Inventory
    {
        [Key]
        public int InventoryID { get; set; }

        [Required]
        public int WarehouseID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double Stock { get; set; }

        public virtual Warehouse WareHouse { get; set; }

        public virtual Product Product { get; set; }
    }
}