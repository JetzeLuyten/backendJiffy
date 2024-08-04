using JiffyBackend.DAL.Entity;

namespace JiffyBackend.API.Dto
{
    public class CreateServiceDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int ServiceTypeId { get; set; }
        public string UserId { get; set; }
        public DateTime PublishDate { get; set; }
        public float Price { get; set; }
    }
}
