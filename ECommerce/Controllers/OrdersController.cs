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
    public class OrdersController : Controller
    {
        private ECommerceDbContext db = new ECommerceDbContext();

        // GET: Orders
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var orders = db.Orders.Where(o => o.CompanyID == user.CompanyID).Include(o => o.Customer).Include(o => o.State);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var order = db.Orders.Find(id);
            
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.CustomerID = new SelectList(ComboHelper.GetCustomers(user.CompanyID), "CustomerID", "FullName");
            var view = new NewOrderView
            {
                Date = DateTime.Now,
                Details = db.OrderDetailTmps.Where(odtmp => odtmp.UserName == User.Identity.Name).ToList(),
            };

            return View(view);
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.CustomerID = new SelectList(ComboHelper.GetCustomers(user.CompanyID), "CustomerID", "FullName");
            return View(order);
        }

        // GET: Orders/AddProducts
        public ActionResult AddProduct()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.ProductID = new SelectList(ComboHelper.GetProducts(user.CompanyID), "ProductID", "Description");
            return View();
        }

        // POST: Orders/AddProducts
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProduct(AddProductView view)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            
            if (ModelState.IsValid)
            {
                var orderDetailTmp = db.OrderDetailTmps.Where(odt => odt.UserName == User.Identity.Name && odt.ProductID == view.ProductID).FirstOrDefault();
                if(orderDetailTmp == null)
                {
                    var product = db.Products.Find(view.ProductID);
                    orderDetailTmp = new OrderDetailTmp
                    {
                        Description = product.Description,
                        Price = product.Price,
                        ProductID = product.ProductID,
                        Quantity = view.Quantity,
                        TaxRate = product.Tax.Rate,
                        UserName = User.Identity.Name
                    };

                    db.OrderDetailTmps.Add(orderDetailTmp);
                }
                else
                {
                    orderDetailTmp.Quantity += view.Quantity;
                    db.Entry(orderDetailTmp).State = EntityState.Modified;
                }

                db.SaveChanges();
                return RedirectToAction("Create");

            }
            ViewBag.ProductID = new SelectList(ComboHelper.GetProducts(user.CompanyID), "ProductID", "Description");
            return View();
        }

        // GET: Orders/DeleteProduct/5
        public ActionResult DeleteProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var orderDetailTmp = db.OrderDetailTmps.Where(odt => odt.UserName == User.Identity.Name && odt.ProductID == id).FirstOrDefault();
            if (orderDetailTmp == null)
            {
                return HttpNotFound();
            }

            db.OrderDetailTmps.Remove(orderDetailTmp);
            db.SaveChanges();

            return RedirectToAction("Create");
        }

        // POST: Orders/DeleteProduct/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProductConfirmed(int id)
        {
            return View();
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "UserName", order.CustomerID);
            ViewBag.StateID = new SelectList(db.States, "StateID", "Description", order.StateID);
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,CustomerID,StateID,Date,Remarks")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "UserName", order.CustomerID);
            ViewBag.StateID = new SelectList(db.States, "StateID", "Description", order.StateID);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
