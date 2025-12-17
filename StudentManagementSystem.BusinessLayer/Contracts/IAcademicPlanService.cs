using StudentManagementSystem.BusinessLayer.DTOs.AcademicPlanDTOs;

namespace StudentManagementSystem.BusinessLayer.Contracts
{
    public interface IAcademicPlanService
    {
        // =====================================
        // Academic Plan CRUD (Admin)
        // =====================================
        Task<ViewAcademicPlanDTO> CreateAcademicPlanAsync(CreateAcademicPlanDTO dto);
        Task<ViewAcademicPlanDTO> UpdateAcademicPlanAsync(UpdateAcademicPlanDTO dto);
        Task<string> DeleteAcademicPlanAsync(string academicPlanId);

        // =====================================
        // Academic Plan Query
        // =====================================
        Task<ViewAcademicPlanDTO> GetAcademicPlanByIdAsync(string academicPlanId);
        Task<IEnumerable<ViewAcademicPlanSummaryDTO>> GetAllAcademicPlansAsync();

        // =====================================
        // Academic Plan Courses Management (Admin)
        // =====================================
        Task<AcademicPlanCourseDTO> AddCourseToAcademicPlanAsync(AddCourseToAcademicPlanDTO dto);
        Task<string> RemoveCourseFromAcademicPlanAsync(string academicPlanId, int courseId);
        Task<IEnumerable<AcademicPlanCourseDTO>> GetCoursesInAcademicPlanAsync(string academicPlanId);

        // =====================================
        // Student Academic Progress
        // =====================================
        Task<StudentAcademicProgressDTO> GetStudentAcademicProgressAsync(string studentUserId);
        Task<StudentAcademicProgressDTO> GetMyAcademicProgressAsync(string userId);

        // =====================================
        // Student Plan Assignment (Admin)
        // =====================================
        Task AssignStudentToAcademicPlanAsync(int studentId, string academicPlanId);
    }
}
