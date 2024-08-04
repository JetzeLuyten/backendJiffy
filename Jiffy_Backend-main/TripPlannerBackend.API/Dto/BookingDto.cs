using JiffyBackend.DAL.Entity;

namespace JiffyBackend.API.Dto
{
    public class BookingDto
    {
        public int Id { get; set; }
        public int BookerId { get; set; }
        public UserDto Booker { get; set; }
        public int ServiceId { get; set; }
        public ServiceDto Service { get; set; }
        public DateTime BookingTime { get; set; }
        public bool Completed { get; set; }
    }
}
