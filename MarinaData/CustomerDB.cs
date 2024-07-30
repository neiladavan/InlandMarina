using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarinaData
{
    /// <summary>
    /// static methods for working with Customer class in InlandMarina database
    /// </summary>
    public static class CustomerDB
    {
        public static Customer Authenticate(InlandMarinaContext db, string username, string password)
        {
            var cusUser = db.Customers.SingleOrDefault(cust => cust.Username == username && cust.Password == password);

            return cusUser!;
        }

        public static void Register(InlandMarinaContext db, Customer newCustomer)
        {
            db.Customers.Add(newCustomer);
            db.SaveChanges();
        }
    }
}
