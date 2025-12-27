using RegMan.Backend.BusinessLayer.DTOs.CartDTOs;
using RegMan.Backend.BusinessLayer.DTOs.EnrollmentDTOs;

namespace RegMan.Backend.BusinessLayer.Contracts;

public interface IEnrollmentService
{
    Task EnrollFromCartAsync(string studentUserId);
    Task<CartCheckoutValidationDTO> ValidateCheckoutFromCartAsync(string studentUserId);
    Task ForceEnrollAsync(string studentUserId, int sectionId);
    Task<int> CountAllAsync();
    Task<IEnumerable<ViewEnrollmentDTO>> GetStudentEnrollmentsAsync(string studentUserId);
}
