using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TutorHub.BusinessLogic.IServices;
using TutorHub.BusinessLogic.Models.Schedules;

namespace TutorHub.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ScheduleController(IScheduleService scheduleService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ScheduleModel>>> GetAllSchedules()
    {
        var schedules =  await scheduleService.GetAllAsync();

        return Ok(schedules);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ScheduleModel>> GetById(int id)
    {
        var schedule = await scheduleService.GetByIdAsync(id);

        return Ok(schedule);
    }

    [HttpPost]
    public async Task<ActionResult<ScheduleModel>> CreateSchedule(ScheduleCreateModel scheduleCreateModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Schedule creation failed due to invalid model.");
        }

        var schedule = await scheduleService.CreateAsync(scheduleCreateModel);

        return Ok(schedule);
    }

    [HttpPut]
    public async Task<ActionResult<ScheduleModel>> UpdateSchedule(ScheduleModel scheduleModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Schedule updating failed due to invalid model.");
        }

        var schedule = await scheduleService.UpdateAsync(scheduleModel);

        return Ok(schedule);
    }

    [HttpDelete]
    public async Task<ActionResult<ScheduleModel>> DeleteSchedule(int id)
    {
        await scheduleService.DeleteAsync(id);

        return NoContent();
    }
}
