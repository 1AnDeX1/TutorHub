using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TutorHub.BusinessLogic.IServices.IUserServices;
using TutorHub.BusinessLogic.Models.User;
using TutorHub.BusinessLogic.Models.User.Teacher;

namespace TutorHub.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class AdminController(IAdminService adminService) : ControllerBase
    {
        [HttpGet("pendingVerificationRequests")]
        public async Task<ActionResult<(IEnumerable<TeacherModel> teacher, int teachersCount)>> GetPendingVerificationRequest
            (string? name, int page = 1, int pageSize = 20)
        {
            var teachersObject = await adminService.GetTeachersWithPendingVerificationRequest(name, page, pageSize);

            return Ok(new { teachersObject.teachers, teachersObject.teachersCount });
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdmin([FromBody] RegistrationModel adminCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Student creation failed due to invalid model.");
            }

            await adminService.CreateAsync(adminCreateModel);

            return CreatedAtAction(nameof(CreateAdmin), new { succeeded = true, message = "Admin registered successfully" });
        }

        [HttpPut("approveVerification/{teacherId}")]
        public async Task<IActionResult> ApproveVerification(int teacherId)
        {
            await adminService.ApproveVerificationRequest(teacherId);

            return NoContent();
        }

        [HttpPut("rejectVerification/{teacherId}")]
        public async Task<IActionResult> RejectVerification(int teacherId)
        {
            await adminService.RejectVerificationRequest(teacherId);

            return NoContent();
        }
    }
}
