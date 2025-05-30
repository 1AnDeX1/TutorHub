using Microsoft.AspNetCore.Mvc;
using TutorHub.BusinessLogic.IServices;
using TutorHub.BusinessLogic.Models.Schedules;

namespace TutorHub.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TeacherAvailabilitiesController(ITeacherAvailabilityService availabilityService) : ControllerBase
{

    [HttpGet("{teacherId}")]
    public async Task<ActionResult<IEnumerable<TeacherAvailabilityModel>?>> GetAvailabilities(int teacherId)
    {
        var availability = await availabilityService.GetByTeacherIdAsync(teacherId);
        return Ok(availability);
    }

    [HttpPost("{teacherId}/add")]
    public async Task<IActionResult> AddAvailability(int teacherId, [FromBody] TeacherAvailabilityRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Availability adding failed due to invalid model.");
        }

        await availabilityService.AddAsync(teacherId, request);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAvailability(int id, [FromBody] UpdateAvailabilityRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Availability updating failed due to invalid model.");
        }

        await availabilityService.UpdateAsync(id, request);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveAvailability(int id)
    {
        await availabilityService.RemoveAsync(id);
        return Ok();
    }
}
