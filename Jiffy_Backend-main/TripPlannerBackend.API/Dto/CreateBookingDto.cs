using JiffyBackend.DAL.Entity;

namespace JiffyBackend.API.Dto
{
    public class CreateBookingDto
    {
        public string BookerAuthId { get; set; }
        public int ServiceId { get; set; }
        public DateTime BookingTime { get; set; }
        public bool Completed { get; set; }
    }
}
