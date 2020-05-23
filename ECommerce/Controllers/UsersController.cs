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
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private ECommerceDbContext db = new ECommerceDbContext();

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.City).Include(u => u.Company).Include(u => u.Department);
            return View(users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(ComboHelper.GetCities(), "CityID", "Name");
            ViewBag.CompanyID = new SelectList(ComboHelper.GetCompanies(), "CompanyID", "Name");
            ViewBag.DepartmentID = new SelectList(ComboHelper.GetDepartments(), "DepartmentID", "Name");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Users.Add(user);
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
                    ViewBag.CityID = new SelectList(ComboHelper.GetCities(), "CityID", "Name", user.CityID);
                    ViewBag.CompanyID = new SelectList(ComboHelper.GetCompanies(), "CompanyID", "Name", user.CompanyID);
                    ViewBag.DepartmentID = new SelectList(ComboHelper.GetDepartments(), "DepartmentID", "Name", user.DepartmentID);
                    return View(user);
                }

                try
                {
                    UserHelper.CreateUserASP(user.UserName, "user", user.UserName);
                }
                catch (Exception ex)
                {
                    db.Users.Remove(user);
                    db.SaveChanges();
                    ModelState.AddModelError(string.Empty, ex.Message);
                    ViewBag.CityID = new SelectList(ComboHelper.GetCities(), "CityID", "Name", user.CityID);
                    ViewBag.CompanyID = new SelectList(ComboHelper.GetCompanies(), "CompanyID", "Name", user.CompanyID);
                    ViewBag.DepartmentID = new SelectList(ComboHelper.GetDepartments(), "DepartmentID", "Name", user.DepartmentID);
                    return View(user);
                }

                try
                {
                    var folder = "~/Content/Users";
                    var file = string.Format("{0}.jpg", user.UserID);

                    if (user.PhotoFile != null)
                    {
                        var response = FileHelper.UploadPhoto(user.PhotoFile, folder, file);
                        if (response)
                        {
                            var picURL = string.Format("{0}/{1}", folder, file);
                            user.Photo = picURL;

                            db.Entry(user).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);
                    ViewBag.CityID = new SelectList(ComboHelper.GetCities(), "CityID", "Name", user.CityID);
                    ViewBag.CompanyID = new SelectList(ComboHelper.GetCompanies(), "CompanyID", "Name", user.CompanyID);
                    ViewBag.DepartmentID = new SelectList(ComboHelper.GetDepartments(), "DepartmentID", "Name", user.DepartmentID);
                    return View(user);
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(ComboHelper.GetCities(), "CityID", "Name", user.CityID);
            ViewBag.CompanyID = new SelectList(ComboHelper.GetCompanies(), "CompanyID", "Name", user.CompanyID);
            ViewBag.DepartmentID = new SelectList(ComboHelper.GetDepartments(), "DepartmentID", "Name", user.DepartmentID);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Users";
                var file = string.Format("{0}.jpg", user.UserID);
                var response = FileHelper.UploadPhoto(user.PhotoFile, folder, file);
                
                if (response)
                {
                    pic = string.Format("{0}/{1}", folder, file);
                    user.Photo = pic;
                }

                var tempDBContext = new ECommerceDbContext();
                var currentUser = tempDBContext.Users.Find(user.UserID);

                if (currentUser.UserName != user.UserName)
                {
                    UserHelper.UpdateUserName(currentUser.UserName, user.UserName);
                }
                
                tempDBContext.Dispose(); //Cerramos la conexión con la base de datos

                db.Entry(user).State = EntityState.Modified;
                //TODO TryCatch
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityID = new SelectList(ComboHelper.GetCities(), "CityID", "Name", user.CityID);
            ViewBag.CompanyID = new SelectList(ComboHelper.GetCompanies(), "CompanyID", "Name", user.CompanyID);
            ViewBag.DepartmentID = new SelectList(ComboHelper.GetDepartments(), "DepartmentID", "Name", user.DepartmentID);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = db.Users.Find(id);
            db.Users.Remove(user);
            //TODO TryCatch
            db.SaveChanges();
            UserHelper.DeleteUser(user.UserName);
            return RedirectToAction("Index");
        }

        public JsonResult GetCities(int departmentId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var cities = db.Cities.Where(c => c.DepartmentID == departmentId);
            return Json(cities);
        }

        public JsonResult GetCompanies(int cityID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var companies = db.Companies.Where(c => c.CityID == cityID);
            return Json(companies);
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
