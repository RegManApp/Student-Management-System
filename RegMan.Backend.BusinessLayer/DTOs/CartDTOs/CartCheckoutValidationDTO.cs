using System;
using System.Collections.Generic;

namespace RegMan.Backend.BusinessLayer.DTOs.CartDTOs
{
    public sealed class CartCheckoutValidationDTO
    {
        public int CartId { get; set; }
        public int ItemCount { get; set; }
        public DateTime ValidatedAtUtc { get; set; }
        public List<CartCheckoutValidationItemDTO> Items { get; set; } = new();
    }

    public sealed class CartCheckoutValidationItemDTO
    {
        public int CartItemId { get; set; }
        public int ScheduleSlotId { get; set; }
        public int SectionId { get; set; }
        public int CourseId { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string Semester { get; set; } = string.Empty;
        public bool SeatsAvailable { get; set; }
    }
}
