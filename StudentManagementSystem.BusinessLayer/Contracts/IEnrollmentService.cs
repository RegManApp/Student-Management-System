using StudentManagementSystem.BusinessLayer.DTOs.EnrollmentDTOs;

namespace StudentManagementSystem.BusinessLayer.Contracts;

public interface IEnrollmentService
{
    Task EnrollFromCartAsync(string studentUserId);
    Task ForceEnrollAsync(string studentUserId, int sectionId);
}
