using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.Classes
{
    public class DBHelper
    {
        public static int GetState(string description, ECommerceDbContext db)
        {
            var state = db.States.Where(s => s.Description == description).FirstOrDefault();
            if (state == null)
            {
                state = new State
                {
                    Description = description
                };
                db.States.Add(state);
                db.SaveChanges();
            }
            return state.StateID;
        }
    }
}