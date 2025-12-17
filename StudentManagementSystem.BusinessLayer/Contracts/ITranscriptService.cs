using StudentManagementSystem.BusinessLayer.DTOs.TranscriptDTOs;

namespace StudentManagementSystem.BusinessLayer.Contracts
{
    public interface ITranscriptService
    {
        // =====================================
        // CRUD Operations (Admin/Instructor)
        // =====================================
        Task<ViewTranscriptDTO> CreateTranscriptAsync(CreateTranscriptDTO dto);
        Task<ViewTranscriptDTO> UpdateGradeAsync(UpdateTranscriptDTO dto);
        Task<string> DeleteTranscriptAsync(int transcriptId);

        // =====================================
        // Query Operations
        // =====================================
        Task<ViewTranscriptDTO> GetTranscriptByIdAsync(int transcriptId);
        Task<IEnumerable<ViewTranscriptDTO>> GetTranscriptsByStudentIdAsync(int studentId);
        Task<IEnumerable<ViewTranscriptDTO>> GetTranscriptsBySemesterAsync(string semester, int year);

        // =====================================
        // Student Operations
        // =====================================
        Task<StudentTranscriptSummaryDTO> GetStudentFullTranscriptAsync(string studentUserId);
        Task<StudentTranscriptSummaryDTO> GetMyTranscriptAsync(string userId);

        // =====================================
        // GPA Calculations
        // =====================================
        Task<double> CalculateStudentGPAAsync(int studentId);
        Task<double> CalculateSemesterGPAAsync(int studentId, string semester, int year);
        Task RecalculateAndUpdateStudentGPAAsync(int studentId);

        // =====================================
        // Bulk Operations (Admin)
        // =====================================
        Task<IEnumerable<ViewTranscriptDTO>> GetAllTranscriptsAsync(
            int? studentId = null,
            int? courseId = null,
            string? semester = null,
            int? year = null,
            string? grade = null);
    }
}
