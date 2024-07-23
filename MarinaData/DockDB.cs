using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarinaData
{
    public static class DockDB
    {
        public static List<Dock> GetDocks(InlandMarinaContext db)
        {
            List<Dock> docks = db.Docks.OrderBy(dock => dock.Name).ToList();

            return docks;
        }
    }
}
