using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.BusinessLayer.Contracts;
using StudentManagementSystem.BusinessLayer.DTOs.ScheduleSlotDTOs;
using StudentManagementSystem.DAL.Contracts;
using StudentManagementSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.BusinessLayer.Services
{
    public class ScheduleSlotService : IScheduleSlotService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IBaseRepository<ScheduleSlot> scheduleSlotRepository;
        private readonly IBaseRepository<TimeSlot> timeSlotRepository;
        private readonly IBaseRepository<Room> roomRepository;
        private readonly IBaseRepository<Section> sectionRepository;
        public ScheduleSlotService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.scheduleSlotRepository = unitOfWork.ScheduleSlots;
            this.roomRepository = unitOfWork.Rooms;
            this.timeSlotRepository = unitOfWork.TimeSlots;
            this.sectionRepository = unitOfWork.Sections;
        }
        public async Task<ViewScheduleSlotDTO> AddScheduleSlotAsync(CreateScheduleSlotDTO scheduleSlot)
        {
            if(scheduleSlot.RoomId==null)
                throw new ArgumentNullException(nameof(scheduleSlot.RoomId), "RoomId cannot be null");
            if(scheduleSlot.TimeSlotId==null)
                throw new ArgumentNullException(nameof(scheduleSlot.RoomId), "TimeSlotId cannot be null");
            if(scheduleSlot.SectionId==null)
                throw new ArgumentNullException(nameof(scheduleSlot.RoomId), "SectionId cannot be null");
            //if theyre not null, check if they exist in db
            Room? room = await roomRepository.GetFilteredAndProjected(
                filter: r => r.RoomId == scheduleSlot.RoomId,
                projection: r => new Room
                {
                    RoomId = r.RoomId,
                    RoomNumber = r.RoomNumber
                }
                ).FirstOrDefaultAsync();
            if (room == null)
                throw new KeyNotFoundException("Room with given RoomId does not exist");
            TimeSlot? timeSlot = await timeSlotRepository.GetFilteredAndProjected(
                filter: t => t.TimeSlotId == scheduleSlot.TimeSlotId,
                projection: t => new TimeSlot
                {
                    TimeSlotId = t.TimeSlotId,
                    Day = t.Day,
                    StartTime = t.StartTime,
                    EndTime = t.EndTime
                }
                ).FirstOrDefaultAsync();
            if (timeSlot == null)
                throw new KeyNotFoundException("TimeSlot with given TimeSlotId does not exist");
           
                //will use this if i wanna get more data from section later like instructor id, name, etc
                //Section? section = await sectionRepository.GetFilteredAndProjected(
                //    filter: s => s.SectionId == scheduleSlot.SectionId,
                //    projection: s => new Section
                //    {
                //        SectionId = s.SectionId
                //    }
                //    ).FirstOrDefaultAsync();
            Section? section = await sectionRepository.GetByIdAsync(scheduleSlot.SectionId);

            if (section == null)
                throw new KeyNotFoundException("Section with given SectionId does not exist");
            //all good, create schedule slot

            ScheduleSlot slot = new ScheduleSlot
            {
                SectionId = scheduleSlot.SectionId,
                RoomId = scheduleSlot.RoomId,
                TimeSlotId = scheduleSlot.TimeSlotId,
                SlotType = scheduleSlot.SlotType
            };
            await scheduleSlotRepository.AddAsync(slot);
            await unitOfWork.SaveChangesAsync();
            return new ViewScheduleSlotDTO
            {
                ScheduleSlotId = slot.ScheduleSlotId,
                SlotType = slot.SlotType,
                SectionId = slot.SectionId,
                RoomId = slot.RoomId,
                RoomNumber =room.RoomNumber,
                TimeSlotId = slot.TimeSlotId,
                Day = timeSlot.Day,
                StartTime=timeSlot.StartTime,
                EndTime=timeSlot.EndTime
            };  
        }
    }
}
