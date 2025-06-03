using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.Tenant;
using RentalManager.Mappings;
using RentalManager.Models;

namespace RentalManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TenantController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetTenants()
        {
            var tenants = await _context.Tenants
                .Include(t => t.User).ThenInclude(u => u.Role)
                .Include(t => t.User).ThenInclude(u => u.Gender)
                .Include(t => t.User).ThenInclude(u => u.UserStatus)
                .Include(t => t.User).ThenInclude(u => u.Property)
                .Include(t => t.Unit)
                .Include(t => t.TenantStatus)
                .ToListAsync();

            if (!tenants.Any())
            {
                return NotFound(new ApiResponse<object>("There are no Tenants available."));
            }

            var tenantDtos = tenants.Select(it => it.ToReadDto()).ToList();

            return Ok(new ApiResponse<List<READTenantDto>>(tenantDtos, ""));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetTenantById(int id)
        {
            var tenant = await _context.Tenants
                .Include(t => t.User).ThenInclude(u => u.Role)
                .Include(t => t.User).ThenInclude(u => u.Gender)
                .Include(t => t.User).ThenInclude(u => u.UserStatus)
                .Include(t => t.User).ThenInclude(u => u.Property)
                .Include(t => t.Unit)
                .Include(t => t.TenantStatus)
                .FirstOrDefaultAsync(pr => pr.Id == id);

            if (tenant == null)
            {
                return NotFound(new ApiResponse<object>("There is no such data"));
            }

            return Ok(new ApiResponse<READTenantDto>(tenant.ToReadDto(), ""));
        }




        [HttpPost]
        public async Task<IActionResult> AddTenant([FromBody] CREATETenantDto AddedTenant)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new ApiResponse<object>("Validation failed.", errors));
            }

            // Validate referenced Unit and Status
            var unit = await _context.Units.FindAsync(AddedTenant.UnitId);
            var status = await _context.SystemCodeItems.FindAsync(AddedTenant.Status);
            var property = await _context.Properties.FindAsync(AddedTenant.User.PropertyId);

            if (status == null || unit == null || property == null)
            {
                return BadRequest(new ApiResponse<object>("Invalid status, unit or property provided."));
            }

            var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Tenant");

            if (defaultRole == null)
            {
                return BadRequest(new ApiResponse<object>("Missing default role"));
            }

            // Begin Transaction
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Create user
                var user = AddedTenant.User.ToEntity(defaultRole.Id);
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Create tenant
                var tenant = AddedTenant.ToEntity(user, unit.Id, status.Id);
                _context.Tenants.Add(tenant);
                await _context.SaveChangesAsync();

                // Commit transaction
                await transaction.CommitAsync();

                // Fetch the fully created tenant with all includes
                var createdTenant = await _context.Tenants
                    .Include(t => t.User).ThenInclude(u => u.Role)
                    .Include(t => t.User).ThenInclude(u => u.Gender)
                    .Include(t => t.User).ThenInclude(u => u.UserStatus)
                    .Include(t => t.User).ThenInclude(u => u.Property)
                    .Include(t => t.Unit)
                    .Include(t => t.TenantStatus)
                    .FirstOrDefaultAsync(t => t.Id == tenant.Id);

                var tenantDto = createdTenant?.ToReadDto();
                return Ok(new ApiResponse<READTenantDto>(tenantDto!, "Tenant created successfully."));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new ApiResponse<object>("An error occurred while creating tenant."));
            }
        }





        [HttpPut("{id}")]
        public async Task<IActionResult> EditTenant(int id, [FromBody] UPDATETenantDto updatedTenant)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new ApiResponse<object>("Validation failed.", errors));
            }

            // Validate referenced Unit and Status
            var unit = await _context.Units.FindAsync(updatedTenant.UnitId);
            var status = await _context.SystemCodeItems.FindAsync(updatedTenant.Status);
            var property = await _context.Properties.FindAsync(updatedTenant.User.PropertyId);

            if (status == null || unit == null || property == null)
            {
                return BadRequest(new ApiResponse<object>("Invalid status, unit or property provided."));
            }


            // Begin Transaction
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {

                // Fetch existing Tenant and related User
                var existintTenant = await _context.Tenants.Include(t => t.User)
                                                   .FirstOrDefaultAsync(t => t.Id == id);

                if (existintTenant == null || existintTenant.User == null)
                {
                    return NotFound(new ApiResponse<object>("Tenant not found."));
                }

                var user = updatedTenant.User.UpdateEntity(existintTenant.User);
                var tenant = updatedTenant.UpdateEntity(existintTenant, existintTenant.User, unit.Id, status.Id);
                await _context.SaveChangesAsync();

                // Commit transaction
                await transaction.CommitAsync();

                // Fetch the fully created tenant with all includes
                var createdTenant = await _context.Tenants
                    .Include(t => t.User).ThenInclude(u => u.Role)
                    .Include(t => t.User).ThenInclude(u => u.Gender)
                    .Include(t => t.User).ThenInclude(u => u.UserStatus)
                    .Include(t => t.User).ThenInclude(u => u.Property)
                    .Include(t => t.Unit)
                    .Include(t => t.TenantStatus)
                    .FirstOrDefaultAsync(t => t.Id == tenant.Id);

                var tenantDto = createdTenant?.ToReadDto();
                return Ok(new ApiResponse<READTenantDto>(tenantDto!, "Tenant updated successfully."));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new ApiResponse<object>(ex.Message));
            }
        }




        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTenant(int id)
        {
            // Begin transaction
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Fetch Tenant with related User
                var tenant = await _context.Tenants
                    .Include(t => t.User)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (tenant == null || tenant.User == null)
                {
                    return NotFound(new ApiResponse<object>(null, $"Tenant with ID {id} was not found."));
                }

                // Remove Tenant first to avoid FK constraint issues
                _context.Tenants.Remove(tenant);
                _context.Users.Remove(tenant.User);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new ApiResponse<object>(null, "Tenant and associated User deleted successfully."));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new ApiResponse<object>(null, $"An error occurred: {ex.Message}"));
            }
        }





    }
}
