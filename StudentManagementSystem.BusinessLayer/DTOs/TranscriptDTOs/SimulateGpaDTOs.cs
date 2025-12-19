namespace StudentManagementSystem.BusinessLayer.DTOs.TranscriptDTOs
{
    public class SimulateCourseDTO
    {
        // Optional: if provided, will replace the transcript with this id
        public int? TranscriptId { get; set; }

        // For new courses (no TranscriptId), credit hours must be provided
        public int? CreditHours { get; set; }

        // Grade string, e.g., "A", "B+"
        public string Grade { get; set; } = string.Empty;
    }

    public class SimulateGpaRequestDTO
    {
        // Optional: Admin/Instructor may provide a studentId. If not provided, controller will resolve current student's id.
        public int? StudentId { get; set; }

        public IEnumerable<SimulateCourseDTO> SimulatedCourses { get; set; } = new List<SimulateCourseDTO>();
    }

    public class SimulateGpaResponseDTO
    {
        public double CurrentGPA { get; set; }
        public double SimulatedGPA { get; set; }
        public double Difference => Math.Round(SimulatedGPA - CurrentGPA, 2);
    }
}
