using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RegMan.Backend.BusinessLayer.Contracts;
using RegMan.Backend.BusinessLayer.DTOs.InstructorDTOs;
using RegMan.Backend.BusinessLayer.DTOs.ScheduleSlotDTOs;
using RegMan.Backend.DAL.Contracts;
using RegMan.Backend.DAL.Entities;

namespace RegMan.Backend.BusinessLayer.Services;

internal class InstructorService : IInstructorService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly UserManager<BaseUser> userManager;

    public InstructorService(
        IUnitOfWork unitOfWork,
        UserManager<BaseUser> userManager)
    {
        this.unitOfWork = unitOfWork;
        this.userManager = userManager;
    }

    // ======================
    // Create Instructor
    // ======================
    public async Task<ViewInstructorDTO> CreateAsync(CreateInstructorDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
            throw new Exception("Instructor title is required.");

        var user = new BaseUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName,
            Address = "N/A",
            Role = "Instructor"
        };

        var result = await userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            throw new Exception("Failed to create instructor user.");

        await userManager.AddToRoleAsync(user, "Instructor");

        var instructor = new InstructorProfile
        {
            UserId = user.Id,
            Title = dto.Title
        };

        await unitOfWork.InstructorProfiles.AddAsync(instructor);
        await unitOfWork.SaveChangesAsync();

        return new ViewInstructorDTO
        {
            InstructorId = instructor.InstructorId,
            FullName = user.FullName,
            Email = user.Email!
        };
    }

    // ======================
    // Get All
    // ======================
    public async Task<IEnumerable<ViewInstructorDTO>> GetAllAsync()
    {
        return await unitOfWork.InstructorProfiles
            .GetAllAsQueryable()
            .Include(i => i.User)
            .Select(i => new ViewInstructorDTO
            {
                InstructorId = i.InstructorId,
                FullName = i.User.FullName,
                Email = i.User.Email!
            })
            .ToListAsync();
    }

    // ======================
    // Get By Id
    // ======================
    public async Task<ViewInstructorDTO> GetByIdAsync(int id)
    {
        var instructor = await unitOfWork.InstructorProfiles
            .GetAllAsQueryable()
            .Include(i => i.User)
            .FirstOrDefaultAsync(i => i.InstructorId == id)
            ?? throw new Exception("Instructor not found.");

        return new ViewInstructorDTO
        {
            InstructorId = instructor.InstructorId,
            FullName = instructor.User.FullName,
            Email = instructor.User.Email!
        };
    }

    // ======================
    // Instructor Schedule
    // ======================
    public async Task<IEnumerable<ViewScheduleSlotDTO>> GetScheduleAsync(int instructorId)
    {
        return await unitOfWork.ScheduleSlots
            .GetAllAsQueryable()
            .Where(s => s.InstructorId == instructorId)
            .Include(s => s.Section)
                .ThenInclude(sec => sec.Course)
            .Include(s => s.Room)
            .Include(s => s.TimeSlot)
            .Include(s => s.Instructor)
                .ThenInclude(i => i.User)
            .Select(s => new ViewScheduleSlotDTO
            {
                ScheduleSlotId = s.ScheduleSlotId,

                SectionId = s.SectionId,
                SectionName = s.Section.Course.CourseName,

                RoomId = s.RoomId,
                Room = s.Room.Building + " - " + s.Room.RoomNumber,

                TimeSlotId = s.TimeSlotId,
                TimeSlot = s.TimeSlot.Day + " " +
                           s.TimeSlot.StartTime + "-" +
                           s.TimeSlot.EndTime,

                InstructorId = s.InstructorId,
                InstructorName = s.Instructor.User.FullName,

                SlotType = s.SlotType.ToString()
            })
            .ToListAsync();
    }
}
