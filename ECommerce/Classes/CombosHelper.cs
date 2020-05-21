using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.Classes
{
    public class CombosHelper : IDisposable
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

        public void Dispose()
        {
            db.Dispose();
        }
    }
}