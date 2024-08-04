using JiffyBackend.DAL.Entity;

namespace JiffyBackend.API.Dto
{
    public class ServiceDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ServiceTypeId { get; set; }

        public ServiceTypeDto ServiceType { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public UserDto User { get; set; }
        public DateTime PublishDate { get; set; }
        public float Price { get; set; }
    }
}
