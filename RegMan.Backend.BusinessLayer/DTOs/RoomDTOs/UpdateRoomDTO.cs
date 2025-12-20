using System.ComponentModel.DataAnnotations;
using RegMan.Backend.DAL.Entities;

namespace RegMan.Backend.BusinessLayer.DTOs.RoomDTOs
{
    public class UpdateRoomDTO
    {
        [Required]
        public int RoomId { get; set; }

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
