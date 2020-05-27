using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.Classes
{
    public class MovementHelper : IDisposable
    {
        private static ECommerceDbContext db = new ECommerceDbContext();

        public static Response NewOrder(NewOrderView view, string userName)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var user = db.Users.Where(u => u.UserName == userName).FirstOrDefault();
                    var order = new Order
                    {
                        CompanyID = user.CompanyID,
                        CustomerID = view.CustomerID,
                        Date = view.Date,
                        Remarks = view.Remarks,
                        StateID = DBHelper.GetState("Created", db)
                    };
                    db.Orders.Add(order);
                    db.SaveChanges();

                    var details = db.OrderDetailTmps.Where(odt => odt.UserName == userName).ToList();
                    foreach (var detail in details)
                    {
                        var orderDetail = new OrderDetail
                        {
                            Description = detail.Description,
                            OrderID = order.OrderID,
                            Price = detail.Price,
                            ProductID = detail.ProductID,
                            Quantity = detail.Quantity,
                            TaxRate = detail.TaxRate
                        };
                        db.OrderDetails.Add(orderDetail);
                        db.OrderDetailTmps.Remove(detail);
                    }
                    db.SaveChanges();
                    transaction.Commit();
                    return new Response { Succeeded = true };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new Response
                    {
                        Message = ex.Message,
                        Succeeded = false
                    };
                }
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}