using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarinaData
{
    /// <summary>
    /// static methods for working with Slip class in InlandMarina database
    /// </summary>
    public static class MarinaDB
    {
        /// <summary>
        /// get all unleased slips
        /// </summary>
        /// <param name="db">context object</param>
        /// <returns>list of unleased slips</returns>
        public static List<Slip> GetSlips(InlandMarinaContext db)
        {
            List<Slip> slips = db.Slips
                .Include(m => m.Dock)
                .Include(m => m.Leases)
                .Where(s => s.Leases.Count == 0)
                .ToList();

            return slips;
        }

        /// <summary>
        /// retrieves unleased slips by givenn dock id
        /// </summary>
        /// <param name="db">context db</param>
        /// <param name="dockId">id of dock for filtering</param>
        /// <returns>filtered slips</returns>
        public static List<Slip> GetSlipsByDockId(InlandMarinaContext db, int dockId)
        {
            List<Slip> slips = db.Slips
                .Include(m => m.Dock)
                .Include(m => m.Leases)
                .Where(s => s.Leases.Count == 0 && s.DockID == dockId)
                .ToList();

            return slips;
        }
    }
}
