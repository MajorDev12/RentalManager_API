using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.Unit;
using RentalManager.DTOs.User;
using RentalManager.Mappings;
using RentalManager.Models;

namespace RentalManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users
                .Include(ub => ub.Property)
                .Include(ub => ub.UserStatus)
                .Include(ub => ub.Gender)
                .Include(ub => ub.Role)
                .ToListAsync();

            if (!users.Any())
            {
                return NotFound(new ApiResponse<object>("There are no Users available."));
            }

            var userDtos = users.Select(it => it.ToReadDto()).ToList();

            return Ok(new ApiResponse<List<READUserDto>>(userDtos, ""));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Users
                .Include(c => c.Property)
                .Include(ub => ub.Gender)
                .Include(ub => ub.UserStatus)
                .Include(ub => ub.Role)
                .FirstOrDefaultAsync(pr => pr.Id == id);

            if (user == null)
            {
                return NotFound(new ApiResponse<object>("There is no such data"));
            }

            return Ok(new ApiResponse<READUserDto>(user.ToReadDto(), ""));
        }



        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] CREATEUserDto AddedUser)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new ApiResponse<object>("Validation failed.", errors));
            }

            var user = AddedUser.ToEntity();

            var status = await _context.SystemCodeItems.FindAsync(AddedUser.UserStatusId);
            var role = await _context.Roles.FindAsync(AddedUser.RoleId);
            var gender = await _context.SystemCodeItems.FindAsync(AddedUser.GenderId);
            var property = await _context.Properties.FindAsync(AddedUser.PropertyId);

            if (status == null || property == null || role == null || gender == null)
            {
                return BadRequest(new ApiResponse<object>("One of the items provided does not exist: status, property, role, gender."));
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var createdUser = await _context.Users
                .Include(c => c.Property)
                .Include(ub => ub.Gender)
                .Include(ub => ub.UserStatus)
                .Include(ub => ub.Role)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            var userDto = createdUser?.ToReadDto();
            return Ok(new ApiResponse<READUserDto>(userDto!, "User added successfully."));
        }




        [HttpPut]
        public async Task<IActionResult> EditUser(int id, [FromBody] UPDATEUserDto dto)
        {

            var user = await _context.Users
                .Include(c => c.Property)
                .Include(ub => ub.Gender)
                .Include(ub => ub.UserStatus)
                .Include(ub => ub.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound(new ApiResponse<object>("User not found."));

            var userDto = dto.UpdateEntity(user);

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<READUserDto>(userDto.ToReadDto(), ""));

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound($"User with ID {id} was not found.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>(null, "Data Deleted Successfully"));
        }


    }
}
