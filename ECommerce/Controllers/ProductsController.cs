
using System;
using System.Collections;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ECommerce.Classes;
using ECommerce.Models;

namespace ECommerce.Controllers
{
    [Authorize(Roles = "User")]
    public class ProductsController : Controller
    {
        private ECommerceDbContext db = new ECommerceDbContext();

        // GET: Products
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var products = db.Products.Include(p => p.Category).Include(p => p.Tax).Where(p => p.CompanyID == user.CompanyID);

            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = db.Products.Find(id);
            
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            
            ViewBag.CategoryID = new SelectList(ComboHelper.GetCategories(user.CompanyID), "CategoryID", "Description");
            ViewBag.TaxID = new SelectList(ComboHelper.GetTaxes(user.CompanyID), "TaxID", "Description");

            var product = new Product { CompanyID = user.CompanyID };

            return View(product);
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            product.CompanyID = user.CompanyID;
            
            if (ModelState.IsValid)
            {
                try
                {
                    db.Products.Add(product);
                    db.SaveChanges();

                    if (product.ImageFile != null)
                    {
                        var folder = "~/Content/Products";
                        var file = string.Format("{0}.jpg", product.ProductID);
                        var response = FileHelper.UploadPhoto(product.ImageFile, folder, file);

                        if (response)
                        {
                            var pic = string.Format("{0}/{1}", folder, file);
                            product.Image = pic;

                            try
                            {
                                db.Entry(product).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                ModelState.AddModelError(string.Empty, ex.Message);
                            }
                        }
                    }
                    else
                    {
                        product.Image = "~/Content/Products/no_img.jpg";
                        try
                        {
                            db.Entry(product).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError(string.Empty, ex.Message);
                        }
                    }
                    return RedirectToAction("Index");
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
                }
                ViewBag.CategoryID = new SelectList(ComboHelper.GetCategories(user.CompanyID), "CategoryID", "Description");
                ViewBag.TaxID = new SelectList(ComboHelper.GetTaxes(user.CompanyID), "TaxID", "Description");
                return View(product);
            }
            ViewBag.CategoryID = new SelectList(ComboHelper.GetCategories(user.CompanyID), "CategoryID", "Description");
            ViewBag.TaxID = new SelectList(ComboHelper.GetTaxes(user.CompanyID), "TaxID", "Description");
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var product = db.Products.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(ComboHelper.GetCategories(product.CompanyID), "CategoryID", "Description", product.CategoryID);
            ViewBag.TaxID = new SelectList(ComboHelper.GetTaxes(product.CompanyID), "TaxID", "Description", product.TaxID);
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            
            if (ModelState.IsValid)
            {
                if (product.ImageFile != null)
                {
                    var folder = "~/Content/Products";
                    var file = string.Format("{0}.jpg", product.ProductID);
                    var response = FileHelper.UploadPhoto(product.ImageFile, folder, file);

                    if (response)
                    {   
                        product.Image = string.Format("{0}/{1}", folder, file);
                    } 
                }
                else
                {
                    product.Image = product.Image;


                }
            }

            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var product = db.Products.Find(id);
            
            if (product == null)
            {
                return HttpNotFound();
            }
            
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var product = db.Products.Find(id);
            
            if(product != null)
            {
                try
                {
                    var response = FileHelper.DeletePhoto(product.Image);
                    
                    if (response)
                    {
                        db.Products.Remove(product);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            ViewBag.CategoryID = new SelectList(ComboHelper.GetCategories(product.CompanyID), "CategoryID", "Description", product.CategoryID);
            ViewBag.TaxID = new SelectList(ComboHelper.GetTaxes(product.CompanyID), "TaxID", "Description", product.TaxID);
            return View(product);
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
