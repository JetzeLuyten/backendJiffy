using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JiffyBackend.API.Dto;
using JiffyBackend.DAL;
using JiffyBackend.DAL.Entity;

namespace JiffyBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly JiffyDbContext _context;
        private readonly IMapper _mapper;
        public ServiceController(JiffyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ServiceDto>>> GetAllServices()
        {
            var services = await _context.Services
        .Include(o => o.ServiceType)
        .Include(o => o.User) // Ensure related User is included
        .ToListAsync();

            if (services == null || !services.Any())
            {
                return NotFound();
            }

            var serviceDtos = _mapper.Map<List<ServiceDto>>(services);
            return Ok(serviceDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceDto>> GetServiceById(int id)
        {
            var service = await _context.Services.Include(o => o.ServiceType).Include(o => o.User).FirstOrDefaultAsync(o => o.Id == id);

            if (service == null)
            {
                return NotFound();
            }

            var serviceDto = _mapper.Map<ServiceDto>(service);
            return Ok(serviceDto);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<ServiceDto>>> GetServicesByUser(string userId)
        {
            var services = await _context.Services
                .Include(o => o.ServiceType)
                .Include(o => o.User)
                .Where(o => o.User.Auth0UserId == userId)
                .ToListAsync();

            if (services == null || !services.Any())
            {
                var offas = await _context.Services
                    .Include(o => o.ServiceType)
                    .Include(o => o.User)
                    .Where(o => o.User.Auth0UserId == "niks")
                    .ToListAsync();
                return Ok(_mapper.Map<List<ServiceDto>>(offas)); //return nothing back
                //return NotFound();
            }

            var serviceDtos = _mapper.Map<List<ServiceDto>>(services);
            return Ok(serviceDtos);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceDto>> CreateService([FromBody] CreateServiceDto createServiceDto)
        {
            if (createServiceDto == null)
            {
                return BadRequest();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0UserId == createServiceDto.UserId);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var service = new Service
            {
                Title = createServiceDto.Title,
                Description = createServiceDto.Description,
                ServiceTypeId = createServiceDto.ServiceTypeId,
                User = user,
                PublishDate = createServiceDto.PublishDate,
                Price = createServiceDto.Price,
            };
            service.User = user;

            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            var createdServiceDto = _mapper.Map<ServiceDto>(service);
            return CreatedAtAction(nameof(GetServiceById), new { id = service.Id }, createdServiceDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(int id, [FromBody] UpdateServiceDto updateServiceDto)
        {
            if (id != updateServiceDto.Id)
            {
                return BadRequest();
            }

            var existingService = await _context.Services.FindAsync(id);
            if (existingService == null)
            {
                return NotFound();
            }

            _mapper.Map(updateServiceDto, existingService);

            _context.Services.Update(existingService);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}