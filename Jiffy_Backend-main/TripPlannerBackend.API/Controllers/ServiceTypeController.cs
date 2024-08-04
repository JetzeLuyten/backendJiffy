using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiffyBackend.DAL.Entity;
using JiffyBackend.DAL;
using AutoMapper;
using JiffyBackend.API.Dto;

namespace JiffyBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceTypeController : ControllerBase
    {
        private readonly JiffyDbContext _context;
        private readonly IMapper _mapper;

        public ServiceTypeController(JiffyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/ServiceType
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceType>>> GetAllServiceTypes()
        {
            var serviceTypes = await _context.ServiceTypes.ToListAsync();

            if (serviceTypes == null || !serviceTypes.Any())
            {
                return NotFound();
            }

            var serviceTypeDtos = _mapper.Map<List<ServiceType>>(serviceTypes);
            return Ok(serviceTypeDtos);
        }

        // GET: api/ServiceType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceType>> GetServiceTypeById(int id)
        {
            var serviceType = await _context.ServiceTypes.FindAsync(id);

            if (serviceType == null)
            {
                return NotFound();
            }

            var serviceTypeDtos = _mapper.Map<ServiceTypeDto>(serviceType);
            return Ok(serviceTypeDtos);
        }

        // PUT: api/ServiceType/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServiceType(int id, [FromBody] UpdateServiceTypeDto updateServiceTypeDto)
        {
            if (id != updateServiceTypeDto.Id)
            {
                return BadRequest();
            }

            var existingServiceType = await _context.ServiceTypes.FindAsync(id);
            if (existingServiceType == null)
            {
                return NotFound();
            }

            _mapper.Map(updateServiceTypeDto, existingServiceType);

            _context.ServiceTypes.Update(existingServiceType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/ServiceType
        [HttpPost]
        public async Task<ActionResult<ServiceTypeDto>> CreateServiceType([FromBody] CreateServiceTypeDto createServiceTypeDto)
        {
            if (createServiceTypeDto == null)
            {
                return BadRequest();
            }

            var serviceType = _mapper.Map<ServiceType>(createServiceTypeDto);

            _context.ServiceTypes.Add(serviceType);
            await _context.SaveChangesAsync();

            var createdServiceTypeDto = _mapper.Map<ServiceTypeDto>(serviceType);
            return CreatedAtAction(nameof(GetServiceTypeById), new { id = serviceType.Id }, createdServiceTypeDto);
        }

        // DELETE: api/ServiceType/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceType(int id)
        {
            var serviceType = await _context.ServiceTypes.FindAsync(id);
            if (serviceType == null)
            {
                return NotFound();
            }

            _context.ServiceTypes.Remove(serviceType);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
