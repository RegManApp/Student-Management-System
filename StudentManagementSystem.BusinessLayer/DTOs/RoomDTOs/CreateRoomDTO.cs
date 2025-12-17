using System.ComponentModel.DataAnnotations;
using StudentManagementSystem.DAL.Entities;

namespace StudentManagementSystem.BusinessLayer.DTOs.RoomDTOs
{
    public class CreateRoomDTO
    {
        [Required]
        [MaxLength(50)]
        public string Building { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string RoomNumber { get; set; } = null!;

        [Range(1, int.MaxValue)]
        public int Capacity { get; set; }

        [Required]
        public RoomType Type { get; set; }
    }
}
