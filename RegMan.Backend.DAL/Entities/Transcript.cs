using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegMan.Backend.DAL.Entities
{
    public class Transcript
    {
        [Key]
        public int TranscriptId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public int SectionId { get; set; }

        [Required]
        [StringLength(10)]
        public string Grade { get; set; } = string.Empty; // A, A-, B+, B, B-, C+, C, C-, D+, D, F

        public double GradePoints { get; set; } // 4.0, 3.7, 3.3, etc.

        [Required]
        public string Semester { get; set; } = string.Empty; // Fall, Spring, Summer

        [Required]
        public int Year { get; set; }

        public int CreditHours { get; set; }

        public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("StudentId")]
        public StudentProfile Student { get; set; } = null!;

        [ForeignKey("CourseId")]
        public Course Course { get; set; } = null!;

        [ForeignKey("SectionId")]
        public Section Section { get; set; } = null!;
    }

    public static class GradeHelper
    {
        private static readonly HashSet<string> GpaGrades = new(StringComparer.OrdinalIgnoreCase)
        {
            "A", "A-", "B+", "B", "B-", "C+", "C", "C-", "D+", "D", "F"
        };

        private static readonly HashSet<string> RecognizedNonGpaGrades = new(StringComparer.OrdinalIgnoreCase)
        {
            "W",   // Withdraw
            "P",   // Pass (non-GPA)
            "NP",  // No Pass (non-GPA)
            "TR",  // Transfer credit
            "T"    // Transfer credit (alt)
        };

        public static double GetGradePoints(string grade)
        {
            if (!CountsTowardGpa(grade))
                return 0.0;

            return grade.ToUpperInvariant() switch
            {
                "A" => 4.0,
                "A-" => 3.7,
                "B+" => 3.3,
                "B" => 3.0,
                "B-" => 2.7,
                "C+" => 2.3,
                "C" => 2.0,
                "C-" => 1.7,
                "D+" => 1.3,
                "D" => 1.0,
                "F" => 0.0,
                _ => 0.0
            };
        }

        public static bool IsValidGrade(string grade)
        {
            return GpaGrades.Contains(grade.Trim());
        }

        public static bool IsRecognizedGrade(string grade)
        {
            if (string.IsNullOrWhiteSpace(grade))
                return false;

            var normalized = grade.Trim();
            return GpaGrades.Contains(normalized) || RecognizedNonGpaGrades.Contains(normalized);
        }

        public static bool CountsTowardGpa(string grade)
        {
            if (string.IsNullOrWhiteSpace(grade))
                return false;

            return GpaGrades.Contains(grade.Trim());
        }

        public static bool IsTransferCredit(string grade)
        {
            if (string.IsNullOrWhiteSpace(grade))
                return false;

            var normalized = grade.Trim();
            return string.Equals(normalized, "TR", StringComparison.OrdinalIgnoreCase)
                || string.Equals(normalized, "T", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsPassing(string grade)
        {
            if (string.IsNullOrWhiteSpace(grade))
                return false;

            var normalized = grade.Trim();

            // Non-GPA grades
            if (string.Equals(normalized, "W", StringComparison.OrdinalIgnoreCase))
                return false;
            if (string.Equals(normalized, "NP", StringComparison.OrdinalIgnoreCase))
                return false;
            if (string.Equals(normalized, "P", StringComparison.OrdinalIgnoreCase))
                return true;
            if (IsTransferCredit(normalized))
                return true;

            // GPA grades
            return GetGradePoints(normalized) >= 1.0; // D or above
        }
    }
}