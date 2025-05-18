using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TutorHub.BusinessLogic.IServices;
using TutorHub.BusinessLogic.IServices.IUserServices;
using TutorHub.BusinessLogic.Models;
using TutorHub.BusinessLogic.Models.Schedules;
using TutorHub.BusinessLogic.Models.User.Teacher;

namespace TutorHub.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeachersController(
        ITeacherService teacherService,
        IScheduleService scheduleService) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<(IEnumerable<TeacherModel> teacher, int teachersCount)>> GetAllTeachers
            (string? name, int page = 1, int pageSize = 20)
        {
            var teachersObject = await teacherService.GetAllAsync(name, page, pageSize);

            return Ok(new { teachersObject.teachers, teachersObject.teachersCount });
        }

        [HttpGet("availableTeachers")]
        public async Task<ActionResult<(IEnumerable<TeacherModel> teacher, int teachersCount)>> GetAvailableTeachers
            (string? name, int page = 1, int pageSize = 20)
        {
            var teachersObject = await teacherService.GetAvailableTeachersAsync(name, page, pageSize);

            return Ok(new { teachersObject.teachers, teachersObject.teachersCount });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TeacherModel>> GetTeacherById(int id)
        {
            var teacher = await teacherService.GetByIdAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }

            return Ok(teacher);
        }

        [HttpGet("{teacherId}/schedule")]
        public async Task<ActionResult<ScheduleModel>> GetScheduleByTeacherId(int teacherId)
        {
            var schedule = await scheduleService.GetByTeacherIdAsync(teacherId);

            return Ok(schedule);
        }

        [HttpPost]
        public async Task<ActionResult<TeacherModel>> CreateTeacher([FromBody] TeacherCreateModel teacherCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Teacher registration failed due to invalid model.");
            }

            await teacherService.CreateAsync(teacherCreateModel);

            return CreatedAtAction(nameof(CreateTeacher), new { succeeded = true, message = "User registered successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeacher(int id, [FromBody] TeacherCreateModel teacherCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Teacher updating failed due to invalid model.");
            }

            await teacherService.UpdateAsync(id, teacherCreateModel);
            return NoContent();
        }

        [HttpPut("requestVerification/{teacherId}")]
        public async Task<IActionResult> RequestVerification(int teacherId)
        {
            await teacherService.SendVerificationRequestAsync(teacherId);

            return NoContent();
        }

        [HttpPost("rate")]
        public async Task<IActionResult> RateTeacher(TeacherRatingModel teacherRatingModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Rating input is invalid.");
            }

            await teacherService.RateTeacherAsync(teacherRatingModel);

            return Ok(new { message = "Teacher was rated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            await teacherService.DeleteAsync(id);
            return NoContent();
        }
    }
}
