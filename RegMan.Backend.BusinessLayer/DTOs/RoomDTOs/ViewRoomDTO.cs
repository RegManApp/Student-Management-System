using RegMan.Backend.DAL.Entities;

namespace RegMan.Backend.BusinessLayer.DTOs.RoomDTOs
{
    public class ViewRoomDTO
    {
        public int RoomId { get; set; }
        public string Building { get; set; } = null!;
        public string RoomNumber { get; set; } = null!;
        public int Capacity { get; set; }
        public RoomType Type { get; set; }
    }
}
