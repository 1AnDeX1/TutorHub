using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TutorHub.BusinessLogic.IServices;
using TutorHub.BusinessLogic.Models.Schedules;
using TutorHub.BusinessLogic.Models.StudentTeacher;
using TutorHub.BusinessLogic.Models.StudentTeachers;
using TutorHub.BusinessLogic.Models.User.Student;
using TutorHub.BusinessLogic.Models.User.Teacher;
using TutorHub.DataAccess.Enums;

namespace TutorHub.WebApi.Controllers;

[Route("api/student-teacher")]
[ApiController]
[Authorize]
public class StudentTeacherController : ControllerBase
{
    private readonly IStudentTeacherService _studentTeacherService;
    private readonly IScheduleService _scheduleService;

    public StudentTeacherController(
        IStudentTeacherService studentTeacherService,
        IScheduleService scheduleService)
    {
        _studentTeacherService = studentTeacherService;
        _scheduleService = scheduleService;
    }

    [HttpGet]
    public async Task<IEnumerable<StudentTeacherSimpleModel>> GetAll()
    {
        var studentTeachers = await _studentTeacherService.GetAllAsync();

        return studentTeachers;
    }

    
    [HttpPost("request")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> RequestStudentToTeacher([FromBody] StudentTeacherRequestModel request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Request sending failed due to invalid model.");
        }

        await _studentTeacherService.RequestStudentToTeacher(request);

        return Ok();
    }


    [HttpPut("confirm")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> ConfirmAttachment(int teacherId, int studentId)
    {
        await _studentTeacherService.ApproveRequestAsync(teacherId, studentId);
        return Ok();
    }

    [HttpPut("reject")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> RejectAttachment(int teacherId, int studentId)
    {
        await _studentTeacherService.RejectRequestAsync(teacherId, studentId);
        return Ok();
    }

    [HttpGet("teachers/{studentId}")]
    [Authorize(Roles = "Student")]
    public async Task<ActionResult<IEnumerable<TeacherModel>>> GetTeachersOfStudentAsync(int studentId, StudentTeacherStatus status)
    {
        var teachers = await _studentTeacherService.GetTeachersOfStudentAsync(studentId, status);
        return Ok(teachers);
    }

    [HttpGet("students/{teacherId}")]
    [Authorize(Roles = "Teacher")]
    public async Task<ActionResult<IEnumerable<StudentModel>>> GetStudentsOfTeacherAsync(int teacherId, StudentTeacherStatus status)
    {
        var students = await _studentTeacherService.GetStudentsOfTeacherAsync(teacherId, status);
        return Ok(students);
    }

    [HttpGet("{studentTeacherId}/schedules")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<ScheduleModel>>> GetschedulesByStudentTeacherId(int studentTeacherId)
    {
        var schedules = await _scheduleService.GetByStudentTeacherIdAsync(studentTeacherId);

        return Ok(schedules);
    }

    [HttpDelete("unsubscribe")]
    public async Task<IActionResult> UnsubscribeStudentFromTeacher(int teacherId, int studentId)
    {
            await _studentTeacherService.DeleteAsync(teacherId, studentId);
            return Ok();
    }
}
