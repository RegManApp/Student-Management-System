using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.API.Common;
using StudentManagementSystem.BusinessLayer.Contracts;
using StudentManagementSystem.BusinessLayer.DTOs.CourseDTOs;

namespace StudentManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService courseService;

        public CourseController(ICourseService courseService)
        {
            this.courseService = courseService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseByIdAsync(int id)
        {
            var course = await courseService.GetCourseByIdAsync(id);

            return Ok(
                ApiResponse<ViewCourseDetailsDTO>
                    .SuccessResponse(course)
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCoursesAsync(
            [FromQuery] string? courseName,
            [FromQuery] int? creditHours,
            [FromQuery] string? courseCode,
            [FromQuery] int? courseCategoryId)
        {
            var courses = await courseService.GetAllCoursesAsync(
                courseName,
                creditHours,
                courseCode,
                courseCategoryId
            );

            return Ok(
                ApiResponse<IEnumerable<ViewCourseSummaryDTO>>
                    .SuccessResponse(courses)
            );
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourseAsync(
            [FromBody] CreateCourseDTO courseDTO)
        {
            var createdCourse = await courseService.CreateCourseAsync(courseDTO);

            return Ok(
                ApiResponse<ViewCourseDetailsDTO>
                    .SuccessResponse(createdCourse, "Course created successfully")
            );
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCourseAsync(
            [FromBody] UpdateCourseDTO courseDTO)
        {
            var updatedCourse = await courseService.UpdateCourseAsync(courseDTO);

            return Ok(
                ApiResponse<ViewCourseDetailsDTO>
                    .SuccessResponse(updatedCourse, "Course updated successfully")
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourseAsync(int id)
        {
            await courseService.DeleteCourseAsync(id);

            return Ok(
                ApiResponse<string>
                    .SuccessResponse("Course deleted successfully")
            );
        }
    }
}
