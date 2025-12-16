using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.API.Common;
using StudentManagementSystem.BusinessLayer.Contracts;
using StudentManagementSystem.BusinessLayer.DTOs.Auth;
using StudentManagementSystem.DAL.Entities;
using System.Security.Claims;
using StudentManagementSystem.BusinessLayer.DTOs.CartDTOs;


namespace StudentManagementSystem.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<BaseUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IAuditLogService auditLogService;

        private readonly ICartService cartService;


        public AdminController(
            UserManager<BaseUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IAuditLogService auditLogService, ICartService cartService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.auditLogService = auditLogService;
            this.cartService = cartService;

        }

        // =========================
        // Helper: Admin Info
        // =========================
        private (string adminId, string adminEmail) GetAdminInfo()
        {
            return (
                User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? throw new Exception("Admin ID not found"),
                User.FindFirstValue(ClaimTypes.Email)
                    ?? "unknown@admin.com"
            );
        }

        // =========================
        // Create User
        // =========================
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO dto)
        {
            if (!await roleManager.RoleExistsAsync(dto.Role))
            {
                return BadRequest(ApiResponse<string>.FailureResponse(
                    "Role does not exist",
                    StatusCodes.Status400BadRequest
                ));
            }

            var user = new BaseUser
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName = dto.Email,
                Address = dto.Address,
                Role = dto.Role
            };

            var result = await userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(ApiResponse<object>.FailureResponse(
                    "User creation failed",
                    StatusCodes.Status400BadRequest,
                    result.Errors.Select(e => e.Description)
                ));
            }

            await userManager.AddToRoleAsync(user, dto.Role);

            // ===== Audit Log =====
            var (adminId, adminEmail) = GetAdminInfo();
            await auditLogService.LogAsync(
                adminId,
                adminEmail,
                "CREATE",
                "User",
                user.Id
            );

            return Ok(ApiResponse<string>.SuccessResponse(
                "User created successfully"
            ));
        }

        [HttpGet("students/{studentId}/cart")]
        public async Task<IActionResult> ViewStudentCart(string studentId)
        {
            var cart = await cartService.ViewCartAsync(studentId);

            return Ok(ApiResponse<ViewCartDTO>.SuccessResponse(cart));
        }

        [HttpPost("students/{studentId}/force-enroll")]
        public async Task<IActionResult> ForceEnroll(string studentId)
        {
            // after adding enrollment logiv
            throw new NotImplementedException("Force enrollment will be implemented in Step 4");
        }


    }
}
