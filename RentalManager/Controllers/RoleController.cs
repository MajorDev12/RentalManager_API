using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.Role;
using RentalManager.DTOs.Unit;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.RoleRepository;
using RentalManager.Services.RoleService;

namespace RentalManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _repo;

        public RoleController(IRoleService context)
        {
            _repo = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var roles = await _repo.GetAll();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            try
            {
                var role = await _repo.GetById(id);
                return Ok(role);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddRole([FromBody] CREATERoleDto AddedRole)
        {

            try
            {
                var property = await _repo.Create(AddedRole);
                return Ok(property);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }




        [HttpPut]
        public async Task<IActionResult> EditRole(int id, [FromBody] UPDATERoleDto dto)
        {

            try
            {
                var role = await _repo.Update(dto);
                return Ok(role);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                var role = await _repo.Delete(id);
                return Ok(role);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
