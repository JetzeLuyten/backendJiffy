using AutoMapper;
using JiffyBackend.API.Dto;
using JiffyBackend.DAL;
using JiffyBackend.DAL.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JiffyBackend.API.Controllers
{
    [Route("/user")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly JiffyDbContext _context;
        private readonly IMapper _mapper;

        public UserController(JiffyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUser()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<UserDto>>(users));
        }

        [HttpGet("{authId}")]
        public async Task<ActionResult<UserDto>> GetUser(string authId)
        {
            if (string.IsNullOrEmpty(authId))
            {
                return BadRequest("Auth ID cannot be null or empty.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0UserId == authId);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            if (createUserDto == null)
            {
                return BadRequest();
            }

            var user = _mapper.Map<User>(createUserDto);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, _mapper.Map<UserDto>(user));
        }

        [HttpPut("{authId}")]
        public async Task<ActionResult> UpdateUserProfile(string authId, [FromBody] UpdateUserDto updates)
        {
            if (string.IsNullOrEmpty(authId))
            {
                return BadRequest("Auth ID cannot be null or empty!");
            }

            if (updates == null)
            {
                return BadRequest("Update data cannot be null.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0UserId == authId);

            if (user == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(updates.FullName))
            {
                user.FullName = updates.FullName;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Update(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
