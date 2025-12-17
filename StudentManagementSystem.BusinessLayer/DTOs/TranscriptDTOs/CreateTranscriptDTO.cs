using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.BusinessLayer.DTOs.TranscriptDTOs
{
    public class CreateTranscriptDTO
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public int SectionId { get; set; }

        [Required]
        [StringLength(10)]
        public string Grade { get; set; } = string.Empty;

        [Required]
        public string Semester { get; set; } = string.Empty;

        [Required]
        public int Year { get; set; }
    }
}
