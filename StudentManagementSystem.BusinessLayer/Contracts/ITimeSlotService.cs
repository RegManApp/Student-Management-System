using StudentManagementSystem.BusinessLayer.DTOs.TimeSlotDTOs;

namespace StudentManagementSystem.BusinessLayer.Contracts
{
    public interface ITimeSlotService
    {
        Task<ViewTimeSlotDTO> CreateTimeSlotAsync(CreateTimeSlotDTO dto);
        Task<bool> DeleteTimeSlotAsync(int timeSlotId);

        Task<IEnumerable<ViewTimeSlotDTO>> GetAllTimeSlotsAsync();
    }
}
