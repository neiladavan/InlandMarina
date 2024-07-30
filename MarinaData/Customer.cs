using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarinaData
{
    [Table("Customer")]
    public class Customer
    {
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        public string LastName { get; set; }

        [Required]
        [StringLength(15)]
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$", ErrorMessage = "Invalid phone number format. Use the format 123-456-7890.")]
        public string Phone { get; set; }

        [Required]
        [StringLength(30)]
        public string City { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(30)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(30)]
        public string Password { get; set; }

        // navigation property
        public virtual ICollection<Lease>? Leases { get; set; }
    }
}
