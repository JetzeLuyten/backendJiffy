using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiffyBackend.DAL.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string Auth0UserId { get; set; } // Auth0 User ID
        public string Email { get; set; }
        public string FullName { get; set; }
        public ICollection<Service> Services { get; set; } = new List<Service>();
        public ICollection<Booking> Bookings { get; set; }
        // Add other fields as necessary
    }
}
