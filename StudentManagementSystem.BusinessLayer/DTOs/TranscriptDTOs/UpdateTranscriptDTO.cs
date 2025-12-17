using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.BusinessLayer.DTOs.TranscriptDTOs
{
    public class UpdateTranscriptDTO
    {
        [Required]
        public int TranscriptId { get; set; }

        [Required]
        [StringLength(10)]
        public string Grade { get; set; } = string.Empty;
    }
}
