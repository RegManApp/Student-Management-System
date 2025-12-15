using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.BusinessLayer.Contracts;
using StudentManagementSystem.DAL.Contracts;
using StudentManagementSystem.DAL.Entities;
using System.Security.Claims;

namespace StudentManagementSystem.BusinessLayer.Services
{
    internal class CartService : ICartService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IBaseRepository<StudentProfile> studentRepository;
        private readonly IBaseRepository<Cart> cartRepository;
        private readonly IBaseRepository<CartItem> cartItemRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        public CartService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            studentRepository = unitOfWork.StudentProfiles;
            cartRepository = unitOfWork.Carts;
            cartItemRepository = unitOfWork.CartItems;
            this.httpContextAccessor = httpContextAccessor;
        }
        // =========================
        // Helpers
        // =========================
        private (string userId, string email) GetUserInfo()
        {
            var user = httpContextAccessor.HttpContext?.User
                ?? throw new Exception("User context not found.");

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new Exception("UserId not found.");

            var email = user.FindFirstValue(ClaimTypes.Email)
                ?? "unknown@email.com";

            return (userId, email);
        }
        public async Task AddToCartAsync(int studentId, int scheduleSlotId)
        {

            //check if valid in db
            StudentProfile? student = await studentRepository.GetByIdAsync(studentId);
            if (student == null)
            {
                throw new InvalidOperationException($"Student with ID {studentId} does not exist.");
            }
            DAL.Entities.Cart? cart = await cartRepository.GetByIdAsync(student.CartId);
            if (cart == null)
            {
                throw new InvalidOperationException($"Cart for student ID {studentId} does not exist.");
            }
            ScheduleSlot? slot = await unitOfWork.ScheduleSlots.GetByIdAsync(scheduleSlotId);
            if (slot == null)
            {
                throw new InvalidOperationException($"Schedule slot with ID {scheduleSlotId} does not exist.");
            }
            CartItem cartItem = new CartItem
            {
                CartId = cart.CartId,
                ScheduleSlotId = scheduleSlotId
            };
            await cartItemRepository.AddAsync(cartItem);
            await unitOfWork.SaveChangesAsync();
        }
        public async Task RemoveFromCartAsync(int studentId, int cartItemId)
        {
            //check if valid in db
            StudentProfile? student = await studentRepository.GetByIdAsync(studentId);
            if (student == null)
            {
                throw new InvalidOperationException($"Student with ID {studentId} does not exist.");
            }
            DAL.Entities.Cart? cart = await cartRepository.GetByIdAsync(student.CartId);
            if (cart == null)
            {
                throw new InvalidOperationException($"Cart for student ID {studentId} does not exist.");
            }
            var cartItem = await cartItemRepository.GetAllAsQueryable().AsNoTracking().Where(ci => ci.CartId == cart.CartId && ci.CartItemId == cartItemId).FirstOrDefaultAsync();
            if (cartItem == null)
            {
                throw new InvalidOperationException($"Cart item with Schedule Slot ID {cartItem.ScheduleSlotId} does not exist in the cart.");
            }
            bool deleted = await cartItemRepository.DeleteAsync(cartItem.CartItemId);
            if (!deleted)
            {
                throw new Exception($"Failed to delete Cart item with ID {cartItem.CartItemId}.");
            }
            await unitOfWork.SaveChangesAsync();
        }
    }
}
