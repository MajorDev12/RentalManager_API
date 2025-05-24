using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.Role;
using RentalManager.DTOs.Unit;
using RentalManager.Mappings;
using RentalManager.Models;

namespace RentalManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RoleController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _context.Roles
                .Include(ub => ub.Property)
                .ToListAsync();

            if (!roles.Any())
            {
                return NotFound(new ApiResponse<object>("There are no Roles available."));
            }

            var roleDtos = roles.Select(it => it.ToReadDto()).ToList();

            return Ok(new ApiResponse<List<READRoleDto>>(roleDtos, ""));
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _context.Roles
                .Include(c => c.Property)
                .FirstOrDefaultAsync(pr => pr.Id == id);

            if (role == null)
            {
                return NotFound(new ApiResponse<object>("There is no such data"));
            }

            return Ok(new ApiResponse<READRoleDto>(role.ToReadDto(), ""));
        }


        [HttpPost]
        public async Task<IActionResult> AddRole([FromBody] CREATERoleDto AddedRole)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new ApiResponse<object>("Validation failed.", errors));
            }

            var role = AddedRole.ToEntity();

            var property = await _context.Properties.FindAsync(AddedRole.PropertyId);

            if (property == null)
            {
                return BadRequest(new ApiResponse<object>("No such Property."));
            }

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            var createdRole = await _context.Roles
                .Include(u => u.Property)
                .FirstOrDefaultAsync(u => u.Id == role.Id);

            var roleDto = createdRole?.ToReadDto();
            return Ok(new ApiResponse<READRoleDto>(roleDto!, "Role added successfully."));
        }




        [HttpPut]
        public async Task<IActionResult> EditRole(int id, [FromBody] UPDATERoleDto dto)
        {

            var role = await _context.Roles
                .Include(c => c.Property)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (role == null)
                return NotFound(new ApiResponse<object>("role not found."));


            // Manual update
            role.Name = dto.Name;
            role.IsEnabled = dto.IsEnabled;
            role.PropertyId = dto.PropertyId;

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<READRoleDto>(role.ToReadDto(), ""));

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                return NotFound($"Role with ID {id} was not found.");
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>(null, "Data Deleted Successfully"));
        }

    }
}
