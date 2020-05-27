using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
    public class State
    {
        [Key]
        public int StateID { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(50, ErrorMessage = "The filed {0} must be maximun {1} characters length")]
        [Display(Name = "State")]
        [Index("State_Description_Index", IsUnique = true)]
        public string Description { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

    }
}