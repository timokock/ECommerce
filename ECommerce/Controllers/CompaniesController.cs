using System;
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
    [Authorize(Roles = "Admin")]
    public class CompaniesController : Controller
    {
        private ECommerceDbContext db = new ECommerceDbContext();

        // GET: Companies
        public ActionResult Index()
        {
            var companies = db.Companies.Include(c => c.City).Include(c => c.Department);
            return View(companies.ToList());
        }

        // GET: Companies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Company company = db.Companies.Find(id);
            
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(ComboHelper.GetCities(), "CityID", "Name");
            ViewBag.DepartmentID = new SelectList(ComboHelper.GetDepartments(), "DepartmentID", "Name");
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Companies.Add(company);
                    db.SaveChanges();
                    var folder = "~/Content/Logos";
                    var file = string.Format("{0}.jpg", company.CompanyID);

                    if (company.LogoFile != null)
                    {
                        var response = FileHelper.UploadPhoto(company.LogoFile, folder, file);

                        if(response)
                        {
                            var pic = string.Format("{0}/{1}", folder, file);
                            company.Logo = pic;

                            try
                            {
                                db.Entry(company).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                ModelState.AddModelError(string.Empty, ex.Message);
                            }
                            ViewBag.CityID = new SelectList(ComboHelper.GetCities(), "CityID", "Name", company.CityID);
                            ViewBag.DepartmentID = new SelectList(ComboHelper.GetDepartments(), "DepartmentID", "Name", company.DepartmentID);
                            return View(company);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null &&
                        ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("_Index"))
                    {
                        ModelState.AddModelError(string.Empty, "This value already exists");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                    ViewBag.CityID = new SelectList(ComboHelper.GetCities(), "CityID", "Name", company.CityID);
                    ViewBag.DepartmentID = new SelectList(ComboHelper.GetDepartments(), "DepartmentID", "Name", company.DepartmentID);
                    return View(company);
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(ComboHelper.GetCities(), "CityID", "Name", company.CityID);
            ViewBag.DepartmentID = new SelectList(ComboHelper.GetDepartments(), "DepartmentID", "Name", company.DepartmentID);
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Logos";
                var file = string.Format("{0}.jpg", company.CompanyID);
                var response = FileHelper.UploadPhoto(company.LogoFile, folder, file);
                
                if (response)
                {
                    pic = string.Format("{0}/{1}", folder, file);
                    company.Logo = pic;
                }
                
                try
                {
                    db.Entry(company).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null &&
                        ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("_Index"))
                    {
                        ModelState.AddModelError(string.Empty, "This value already exists");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                    ViewBag.CityID = new SelectList(ComboHelper.GetCities(), "CityID", "Name", company.CityID);
                    ViewBag.DepartmentID = new SelectList(ComboHelper.GetDepartments(), "DepartmentID", "Name", company.DepartmentID);
                    return View(company);
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.Companies.Find(id);
            
            try
            {
                db.Companies.Remove(company);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null &&
                   ex.InnerException.InnerException != null &&
                   ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    ModelState.AddModelError(string.Empty, "The record can not be deleted because it has related records");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                return View(company);
            }
            return RedirectToAction("Index");
        }

        public JsonResult GetCities(int departmentId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var cities = db.Cities.Where(c => c.DepartmentID == departmentId);
            return Json(cities);
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
