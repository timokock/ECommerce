using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ECommerce.Classes;
using ECommerce.Models;

namespace ECommerce.Controllers
{
    [Authorize(Roles = "User")]
    public class CustomersController : Controller
    {
        private ECommerceDbContext db = new ECommerceDbContext();

        // GET: Customers
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var customers = db.Customers.Where(c => c.CompanyID == user.CompanyID).Include(c => c.City).Include(c => c.Department);
            return View(customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var customer = db.Customers.Find(id);
            
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(ComboHelper.GetCities(), "CityID", "Name");
            ViewBag.DepartmentID = new SelectList(ComboHelper.GetDepartments(), "DepartmentID", "Name");
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var customer = new Customer { CompanyID = user.CompanyID };
            return View(customer);
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                UserHelper.CreateUserASP(customer.UserName, "Customer");
                return RedirectToAction("Index");
            }

            ViewBag.CityID = new SelectList(ComboHelper.GetCities(), "CityID", "Name", customer.CityID);
            ViewBag.DepartmentID = new SelectList(ComboHelper.GetDepartments(), "DepartmentID", "Name", customer.DepartmentID);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(ComboHelper.GetCities(), "CityID", "Name", customer.CityID);
            ViewBag.DepartmentID = new SelectList(ComboHelper.GetDepartments(), "DepartmentID", "Name", customer.DepartmentID);
            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                // TODO: Validate when the customoer email changes
                return RedirectToAction("Index");
            }
            ViewBag.CityID = new SelectList(ComboHelper.GetCities(), "CityID", "Name", customer.CityID);
            ViewBag.DepartmentID = new SelectList(ComboHelper.GetDepartments(), "DepartmentID", "Name", customer.DepartmentID);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var customer = db.Customers.Find(id);
            
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            UserHelper.DeleteUser(customer.UserName);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
