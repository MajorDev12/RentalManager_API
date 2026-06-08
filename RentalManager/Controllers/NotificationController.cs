using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentalManager.Authorization.Permissions;
using RentalManager.Authorization.Policies;
using RentalManager.Helpers.Authorization;

namespace RentalManager.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationService _service;
        public NotificationController(NotificationService service)
        {
            _service = service;
        }


        [Authorize(Policy = PermissionNames.Notification.Read)]
        [HttpGet("UnRead")]
        public async Task<IActionResult> GetUnread([FromQuery] int userId)
        {
            try
            {
                var result = await _service.GetUnRead(userId);

                if (result.Success == false) return BadRequest(result);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = PermissionNames.Notification.Read)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAll();

                if (result.Success == false) return BadRequest(result);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
