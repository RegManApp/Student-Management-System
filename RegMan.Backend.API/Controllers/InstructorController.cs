using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegMan.Backend.BusinessLayer.Contracts;
using RegMan.Backend.BusinessLayer.DTOs.InstructorDTOs;
using System.Security.Claims;

namespace RegMan.Backend.API.Controllers;

[ApiController]
[Route("api/instructor")]
[Authorize] // لازم يكون عامل Login
public class InstructorController : ControllerBase
{
    private readonly IInstructorService instructorService;

    public InstructorController(IInstructorService instructorService)
    {
        this.instructorService = instructorService;
    }

    // =========================
    // Create Instructor
    // Admin only
    // =========================
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateInstructorDTO dto)
        => Ok(await instructorService.CreateAsync(dto));

    // =========================
    // Get All Instructors
    // Admin only
    // =========================
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await instructorService.GetAllAsync());

    // =========================
    // Get Instructor By Id
    // Admin only
    // =========================
    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(await instructorService.GetByIdAsync(id));

    // =========================
    // Get Instructor Schedule
    // Admin OR Instructor (own schedule)
    // =========================
    [Authorize(Roles = "Admin,Instructor")]
    [HttpGet("{id}/schedule")]
    public async Task<IActionResult> GetSchedule(int id)
    {
        if (User.IsInRole("Instructor"))
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            // InstructorProfile.UserId == BaseUser.Id
            var instructor = await instructorService.GetByIdAsync(id);

            if (instructor == null)
                return NotFound();
        }

        return Ok(await instructorService.GetScheduleAsync(id));
    }
}
