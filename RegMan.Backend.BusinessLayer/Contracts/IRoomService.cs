using RegMan.Backend.BusinessLayer.DTOs.RoomDTOs;

namespace RegMan.Backend.BusinessLayer.Contracts
{
    public interface IRoomService
    {
        Task<ViewRoomDTO> CreateRoomAsync(CreateRoomDTO dto);
        Task<ViewRoomDTO> UpdateRoomAsync(UpdateRoomDTO dto);
        Task<bool> DeleteRoomAsync(int roomId);

        Task<ViewRoomDTO> GetRoomByIdAsync(int roomId);
        Task<IEnumerable<ViewRoomDTO>> GetAllRoomsAsync();
    }
}
