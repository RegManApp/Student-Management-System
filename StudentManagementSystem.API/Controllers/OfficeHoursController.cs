using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.API.Common;
using StudentManagementSystem.BusinessLayer.Contracts;
using StudentManagementSystem.BusinessLayer.DTOs.OfficeHoursDTOs;

namespace StudentManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeHoursController : ControllerBase
    {
        private readonly IOfficeHoursService officeHoursService;
        public OfficeHoursController(IOfficeHoursService officeHoursService)
        {
            this.officeHoursService = officeHoursService;
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetOfficeHoursById(int id) 
        {
            var response = await officeHoursService.GetOfficeHoursByInstructorIdAsync(id);
            return Ok(ApiResponse<List<ViewOfficeHoursDTO>>.SuccessResponse(response));
        }
    }
}
