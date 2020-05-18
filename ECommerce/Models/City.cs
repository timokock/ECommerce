using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class City
    {
        [Key]
        public int CityID { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(50, ErrorMessage = "The field {0} must be maximum {1} characters length")]
        [Display(Name = "City")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        public int DepartmentID { get; set; }

        public virtual Department Department { get; set; }


    }
}