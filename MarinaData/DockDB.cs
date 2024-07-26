using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarinaData
{
    /// <summary>
    /// static methods for working with Dock class in InlandMarina database
    /// </summary>
    public static class DockDB
    {
        /// <summary>
        /// get all docks in alphabetical order
        /// </summary>
        /// <param name="db">context db</param>
        /// <returns>list of all docks</returns>
        public static List<Dock> GetDocks(InlandMarinaContext db)
        {
            List<Dock> docks = db.Docks.OrderBy(dock => dock.Name).ToList();

            return docks;
        }
    }
}
