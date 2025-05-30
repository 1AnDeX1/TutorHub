using Microsoft.AspNetCore.Mvc;
using TutorHub.BusinessLogic.FilterPipe;
using TutorHub.BusinessLogic.IServices;
using TutorHub.BusinessLogic.IServices.IUserServices;
using TutorHub.BusinessLogic.Models;
using TutorHub.BusinessLogic.Models.Chat;
using TutorHub.BusinessLogic.Models.Schedules;
using TutorHub.BusinessLogic.Models.User.Teacher;
using TutorHub.BusinessLogic.Models.User.Teachers;

namespace TutorHub.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeachersController(
    ITeacherService teacherService,
    IScheduleService scheduleService,
    IChatService chatService) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<(IEnumerable<TeacherModel> teacher, int teachersCount)>> GetAllTeachers
        (string? name, int page = 1, int pageSize = 20)
    {
        var teachersObject = await teacherService.GetAllAsync(name, page, pageSize);

        return Ok(new { teachersObject.teachers, teachersObject.teachersCount });
    }

    [HttpPost("availableTeachers")]
    public async Task<ActionResult<PagedTeacherResult>> GetAvailableTeachers([FromBody] TeacherFilter filter)
    {
        var teachersObject = await teacherService.GetAvailableTeachersAsync(filter);
        return Ok(teachersObject);
    }

    [HttpPost("bestTeachers")]
    public async Task<ActionResult<PagedTeacherResult>> GetBestTeachersAsync(TeacherFilter filter)
    {
        var teachersObject = await teacherService.GetBestTeachersAsync(filter);

        return Ok(teachersObject);
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
    public async Task<ActionResult<TeacherScheduleModel>> GetScheduleByTeacherId(int teacherId)
    {
        var schedule = await scheduleService.GetByTeacherIdAsync(teacherId);

        return Ok(schedule);
    }

    [HttpGet("chats/{teacherId}")]
    public async Task<ActionResult<IEnumerable<ChatModel>>> GetChatsByTeacherId(int teacherId)
    {
        var chat = await chatService.GetAllByTeacherIdAsync(teacherId);

        if (chat == null)
            return NotFound();

        return Ok(chat);
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
