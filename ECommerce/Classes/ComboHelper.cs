using ECommerce.Models;
using System;
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

        public void Dispose()
        {
            db.Dispose();
        }
    }
}