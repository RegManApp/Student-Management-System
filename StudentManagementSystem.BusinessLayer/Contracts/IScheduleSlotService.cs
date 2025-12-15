using StudentManagementSystem.BusinessLayer.DTOs.ScheduleSlotDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.BusinessLayer.Contracts
{
    public interface IScheduleSlotService
    {
        Task<ViewScheduleSlotDTO> AddScheduleSlotAsync(CreateScheduleSlotDTO scheduleSlot);
    }
}
