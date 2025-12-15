using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.API.Common;
using StudentManagementSystem.BusinessLayer.Contracts;
using System.Security.Claims;

namespace StudentManagementSystem.API.Controllers
{
    [Authorize(Roles ="Student")]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;
        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }
        // Add To Cart
        [HttpPost("add")]
        public async Task<IActionResult> AddToCartAsync([FromQuery] int scheduleSlotId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userId == null)
            {
                return Unauthorized(ApiResponse<string>.FailureResponse(
                    "Unauthorized",
                    StatusCodes.Status401Unauthorized
                ));
            }
            try
            {
                await cartService.AddToCartAsync(userId, scheduleSlotId);
                return Ok(new { Message = "Item added to cart successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.FailureResponse(
                    ex.Message,
                    StatusCodes.Status400BadRequest
                ));
            }

        }
        // Remove From Cart
        [HttpDelete("{cartItemId}")]
        public async Task<IActionResult> RemoveFromCartAsync(int cartItemId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userId == null)
            {
                return Unauthorized(ApiResponse<string>.FailureResponse(
                    "Unauthorized",
                    StatusCodes.Status401Unauthorized
                ));
            }
            try
            {
                await cartService.AddToCartAsync(userId, cartItemId);
                return Ok(new { Message = "Item removed from cart successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.FailureResponse(
                    ex.Message,
                    StatusCodes.Status400BadRequest
                ));
            }
        }
    }
}
