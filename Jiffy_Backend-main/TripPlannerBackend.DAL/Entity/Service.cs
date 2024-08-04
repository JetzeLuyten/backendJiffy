using System;

namespace JiffyBackend.DAL.Entity
{
    public class Service
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ServiceTypeId { get; set; } // Foreign key property
        public ServiceType ServiceType { get; set; } // Navigation property
        public string Description { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime PublishDate { get; set; }
        public List<Booking> Bookings { get; set; } = new List<Booking>();
        public float Price { get; set; }
    }
}
