using Microsoft.AspNetCore.Mvc;
using TutorHub.BusinessLogic.Models.User.Student;
using TutorHub.BusinessLogic.IServices.IUserServices;
using TutorHub.BusinessLogic.Models.Schedules;
using TutorHub.BusinessLogic.IServices;
using Microsoft.AspNetCore.Authorization;

namespace TutorHub.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentsController(
        IStudentService studentService,
        IScheduleService scheduleService) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<(IEnumerable<StudentModel> students, int studentsCount)>> GetAllStudents
            (string? name, int page = 1, int pageSize = 20)
        {
            var studentsObject = await studentService.GetAllAsync(name, page, pageSize);
            return Ok(new { studentsObject.students, studentsObject.studentsCount });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentModel>> GetStudentById(int id)
        {
            var student = await studentService.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        [HttpGet("{studentId}/schedule")]
        public async Task<ActionResult<ScheduleModel>> GetScheduleByStudentId(int studentId)
        {
            var schedule = await scheduleService.GetByStudentIdAsync(studentId);

            return Ok(schedule);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateStudent([FromBody] StudentCreateModel studentCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Student creation failed due to invalid model.");
            }

            await studentService.CreateAsync(studentCreateModel);

            return CreatedAtAction(nameof(CreateStudent), new { succeeded = true, message = "User registered successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentCreateModel studentCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Student updating failed due to invalid model.");
            }

            _ = await studentService.UpdateAsync(id, studentCreateModel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            await studentService.DeleteAsync(id);
            return NoContent();
        }
    }
}
