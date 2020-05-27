using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        [Display(Name = "Company")]
        public int CompanyID { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(256, ErrorMessage = "The filed {0} must be maximun {1} characters length")]
        [Display(Name = "E-Mail")]
        [Index("Customer_UserName_Index", IsUnique = true)]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(50, ErrorMessage = "The filed {0} must be maximun {1} characters length")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(50, ErrorMessage = "The filed {0} must be maximun {1} characters length")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(20, ErrorMessage = "The filed {0} must be maximun {1} characters length")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(100, ErrorMessage = "The filed {0} must be maximun {1} characters length")]
        public string Address { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        [Display(Name = "Department")]
        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        [Display(Name = "City")]
        public int CityID { get; set; }

        [Display(Name = "Customer")]
        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }

        public virtual Department Department { get; set; }

        public virtual City City { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

    }
}