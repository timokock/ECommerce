using ECommerce.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ECommerce.Classes
{
    public class ComboHelper : IDisposable
    {
        private static ECommerceDbContext db = new ECommerceDbContext();

        public static List<Department> GetDepartments()
        {
            var departments = db.Departments.ToList();
            departments.Add(new Department
            {
                DepartmentID = 0,
                Name = "[Select a department...]"
            });
            return departments = departments.OrderBy(d => d.Name).ToList();
        }

        public static List<Product> GetProducts(int companyID)
        {
            var product = db.Products.Where(p => p.CompanyID == companyID).ToList();
            product.Add(new Product
            {
                ProductID = 0,
                Description = "[Select a product...]"
            });
            return product = product.OrderBy(p => p.Description).ToList();
        }

        public static List<City> GetCities()
        {
            var cities = db.Cities.ToList();
            cities.Add(new City
            {
                CityID = 0,
                Name = "[Select a city...]"
            });
            return cities = cities.OrderBy(d => d.Name).ToList();
        }

        public static List<Company> GetCompanies()
        {
            var companies = db.Companies.ToList();
            companies.Add(new Company
            {
                CompanyID = 0,
                Name = "[Select a company...]"
            });
            return companies = companies.OrderBy(d => d.Name).ToList();
        }

        public static List<Category> GetCategories(int companyID)
        {
            var categories = db.Categories.Where(c => c.CompanyID == companyID).ToList();
            categories.Add(new Category
            {
                CategoryID = 0,
                Description = "[Select a category...]"
            });
            return categories = categories.OrderBy(d => d.Description).ToList();
        }

        public static List<Customer> GetCustomers(int companyID)
        {
            var customer = db.Customers.Where(c => c.CompanyID == companyID).ToList();
            customer.Add(new Customer
            {
                CustomerID = 0,
                FirstName = "[Select a customer...]"
            });
            return customer = customer.OrderBy(c => c.FirstName).ThenBy(c => c.LastName).ToList();
        }

        public static List<Tax> GetTaxes(int companyID)
        {
            var taxes = db.Taxes.Where(t => t.CompanyID == companyID).ToList();
            taxes.Add(new Tax
            {
                TaxID = 0,
                Description = "[Select a tax...]"
            });
            return taxes = taxes.OrderBy(d => d.Description).ToList();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}