using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class Company
    {
        [Key]
        public int CompanyID { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(50, ErrorMessage = "The field {0} must be maximum {1} characters length")]
        [Display(Name = "Company")]
        [Index("Department_Name_Index", IsUnique = true)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(20, ErrorMessage = "The field {0} must be maximum {1} numbers length")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(100, ErrorMessage = "The field {0} must be maximum {1} characters length")]
        public string Address { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Logo { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public int CityID { get; set; }

        [NotMapped] // No mandamos a base de datos
        public HttpPostedFileBase LogoFile { get; set; }

        public virtual Department Department { get; set; }

        public virtual City City { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Category> Categories { get; set; }

        public virtual ICollection<Tax> Taxes { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Warehouse> Warehouses { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}