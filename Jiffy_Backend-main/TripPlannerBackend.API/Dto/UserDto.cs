namespace JiffyBackend.API.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Auth0UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
    }
}
