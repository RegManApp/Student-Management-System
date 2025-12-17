using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.BusinessLayer.Contracts;
using StudentManagementSystem.BusinessLayer.DTOs.SectionDTOs;

namespace StudentManagementSystem.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private readonly ISectionService sectionService;

        public SectionController(ISectionService sectionService)
        {
            this.sectionService = sectionService;
        }

        // =========================
        // CREATE
        // Admin only
        // =========================
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateSectionAsync(CreateSectionDTO sectionDTO)
        {
            var result = await sectionService.CreateSectionAsync(sectionDTO);
            return Ok(result);
        }

        // =========================
        // GET BY ID
        // Admin + Instructor + Student
        // =========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSectionByIdAsync(int id)
        {
            var result = await sectionService.GetSectionByIdAsync(id);
            return Ok(result);
        }

        // =========================
        // GET ALL
        // Admin + Instructor + Student
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetAllSectionsAsync(
            string? semester,
            DateTime? year,
            int? instructorId,
            int? courseId,
            int? seats)
        {
            var result = await sectionService.GetAllSectionsAsync(
                semester,
                year,
                instructorId,
                courseId,
                seats);

            return Ok(result);
        }

        // =========================
        // UPDATE
        // Admin only
        // =========================
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateSectionAsync(UpdateSectionDTO sectionDTO)
        {
            var result = await sectionService.UpdateSectionAsync(sectionDTO);
            return Ok(result);
        }

        // =========================
        // DELETE
        // Admin only
        // =========================
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSectionAsync(int id)
        {
            var result = await sectionService.DeleteSectionAsync(id);
            return Ok(result);
        }
    }
}
