using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiffyBackend.DAL.Entity
{
    public class Booking
    {
        public int Id { get; set; }
        public int BookerId { get; set; }
        public User Booker { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
        public DateTime BookingTime { get; set; }
        public bool Complete { get; set; }
    }
}
