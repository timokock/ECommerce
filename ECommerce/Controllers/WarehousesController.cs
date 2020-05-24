using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ECommerce.Classes;
using ECommerce.Models;

namespace ECommerce.Controllers
{
    [Authorize(Roles = "User")]
    public class WarehousesController : Controller
    {
        private ECommerceDbContext db = new ECommerceDbContext();

        // GET: Warehouses
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var warehouses = db.Warehouses.Where(w => w.CompanyID == user.CompanyID).Include(w => w.City).Include(w => w.Department);
            return View(warehouses.ToList());
        }

        // GET: Warehouses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var warehouse = db.Warehouses.Find(id);
            
            if (warehouse == null)
            {
                return HttpNotFound();
            }
            return View(warehouse);
        }

        // GET: Warehouses/Create
        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(ComboHelper.GetCities(), "CityID", "Name");
            ViewBag.DepartmentID = new SelectList(ComboHelper.GetDepartments(), "DepartmentID", "Name");
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var warehouse = new Warehouse { CompanyID = user.CompanyID };
            return View(warehouse);
        }

        // POST: Warehouses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WarehouseID,CompanyID,Name,Phone,Address,DepartmentID,CityID")] Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                db.Warehouses.Add(warehouse);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityID = new SelectList(ComboHelper.GetCities(), "CityID", "Name", warehouse.CityID);
            ViewBag.DepartmentID = new SelectList(ComboHelper.GetDepartments(), "DepartmentID", "Name", warehouse.DepartmentID);
            return View(warehouse);
        }

        // GET: Warehouses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var warehouse = db.Warehouses.Find(id);
            
            if (warehouse == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.CityID = new SelectList(ComboHelper.GetCities(), "CityID", "Name", warehouse.CityID);
            ViewBag.DepartmentID = new SelectList(ComboHelper.GetDepartments(), "DepartmentID", "Name", warehouse.DepartmentID);
            return View(warehouse);
        }

        // POST: Warehouses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(warehouse).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityID = new SelectList(ComboHelper.GetCities(), "CityID", "Name", warehouse.CityID);
            ViewBag.DepartmentID = new SelectList(ComboHelper.GetDepartments(), "DepartmentID", "Name", warehouse.DepartmentID);
            return View(warehouse);
        }

        // GET: Warehouses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var warehouse = db.Warehouses.Find(id);
            
            if (warehouse == null)
            {
                return HttpNotFound();
            }
            
            return View(warehouse);
        }

        // POST: Warehouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Warehouse warehouse = db.Warehouses.Find(id);
            db.Warehouses.Remove(warehouse);
            db.SaveChanges();
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
