using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.BusinessLayer.Contracts;
using StudentManagementSystem.BusinessLayer.DTOs.CourseDTOs;
using StudentManagementSystem.BusinessLayer.DTOs.ScheduleSlotDTOs;
using StudentManagementSystem.BusinessLayer.DTOs.SectionDTOs;
using StudentManagementSystem.DAL.Contracts;
using StudentManagementSystem.DAL.Entities;

namespace StudentManagementSystem.BusinessLayer.Services
{
    internal class SectionService : ISectionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IBaseRepository<Section> sectionRepository;
        private readonly IBaseRepository<Course> courseRepository;
        private readonly IBaseRepository<InstructorProfile> instructorRepository;
        private readonly ICourseService courseService;

        public SectionService(IUnitOfWork unitOfWork, ICourseService courseService)
        {
            this.unitOfWork = unitOfWork;
            this.sectionRepository = unitOfWork.Sections;
            this.courseRepository = unitOfWork.Courses;
            this.instructorRepository = unitOfWork.InstructorProfiles;
            this.courseService = courseService;
        }

        // =========================
        // Create
        // =========================
        public async Task<ViewSectionDTO> CreateSectionAsync(CreateSectionDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var course = await courseRepository.GetByIdAsync(dto.CourseId)
                ?? throw new Exception("Course not found");

            string? instructorName = null;
            if (dto.InstructorId.HasValue)
            {
                var instructor = await instructorRepository
                    .GetAllAsQueryable()
                    .Include(i => i.User)
                    .FirstOrDefaultAsync(i => i.InstructorId == dto.InstructorId);

                if (instructor == null)
                    throw new Exception("Instructor not found");

                instructorName = instructor.User.FullName;
            }

            var section = new Section
            {
                Semester = dto.Semester,
                Year = dto.Year,
                CourseId = dto.CourseId,
                InstructorId = dto.InstructorId,
                AvailableSeats = dto.AvailableSeats
            };

            await sectionRepository.AddAsync(section);
            await unitOfWork.SaveChangesAsync();

            return new ViewSectionDTO
            {
                SectionId = section.SectionId,
                Semester = section.Semester,
                Year = section.Year,
                InstructorId = section.InstructorId,
                InstructorName = instructorName,
                AvailableSeats = section.AvailableSeats,
                CourseSummary = await courseService.GetCourseSummaryByIdAsync(section.CourseId),
                ScheduleSlots = Enumerable.Empty<ViewScheduleSlotDTO>()
            };
        }

        // =========================
        // Get By Id
        // =========================
        public async Task<ViewSectionDTO> GetSectionByIdAsync(int id)
        {
            var section = await BuildBaseQuery()
                .FirstOrDefaultAsync(s => s.SectionId == id);

            if (section == null)
                throw new Exception($"Section with ID {id} not found");

            return section;
        }

        // =========================
        // Get All
        // =========================
        public async Task<IEnumerable<ViewSectionDTO>> GetAllSectionsAsync(
            string? semester,
            DateTime? year,
            int? instructorId,
            int? courseId,
            int? seats)
        {
            var query = sectionRepository
                .GetAllAsQueryable()
                .Include(s => s.Course)
                .Include(s => s.Instructor)
                    .ThenInclude(i => i.User)
                .Include(s => s.Slots)
                    .ThenInclude(sl => sl.Room)
                .Include(s => s.Slots)
                    .ThenInclude(sl => sl.TimeSlot)
                .Include(s => s.Slots)
                    .ThenInclude(sl => sl.Instructor)
                        .ThenInclude(i => i.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(semester))
                query = query.Where(s => s.Semester == semester);

            if (year.HasValue)
                query = query.Where(s => s.Year == year.Value);

            if (instructorId.HasValue)
                query = query.Where(s => s.InstructorId == instructorId.Value);

            if (courseId.HasValue)
                query = query.Where(s => s.CourseId == courseId.Value);

            if (seats.HasValue)
                query = query.Where(s => s.AvailableSeats == seats.Value);

            return await query
                .Select(s => new ViewSectionDTO
                {
                    SectionId = s.SectionId,
                    Semester = s.Semester,
                    Year = s.Year,
                    InstructorId = s.InstructorId,
                    InstructorName = s.Instructor != null
                        ? s.Instructor.User.FullName
                        : null,
                    AvailableSeats = s.AvailableSeats,

                    CourseSummary = new ViewCourseSummaryDTO
                    {
                        CourseId = s.Course.CourseId,
                        CourseName = s.Course.CourseName,
                        CourseCode = s.Course.CourseCode,
                        CreditHours = s.Course.CreditHours
                    },

                    ScheduleSlots = s.Slots.Select(slot => new ViewScheduleSlotDTO
                    {
                        ScheduleSlotId = slot.ScheduleSlotId,
                        SectionId = slot.SectionId,
                        SectionName =
                            s.Course.CourseName + " - Section " + s.SectionId,

                        RoomId = slot.RoomId,
                        Room =
                            slot.Room.Building + " - " + slot.Room.RoomNumber,

                        TimeSlotId = slot.TimeSlotId,
                        TimeSlot =
                            slot.TimeSlot.Day + " " +
                            slot.TimeSlot.StartTime + "-" +
                            slot.TimeSlot.EndTime,

                        InstructorId = slot.InstructorId,
                        InstructorName = slot.Instructor.User.FullName,
                        SlotType = slot.SlotType.ToString()
                    }).ToList()
                })
                .ToListAsync();
        }

        // =========================
        // Update
        // =========================
        public async Task<ViewSectionDTO> UpdateSectionAsync(UpdateSectionDTO dto)
        {
            var section = await sectionRepository.GetByIdAsync(dto.SectionId)
                ?? throw new Exception("Section not found");

            section.Semester = dto.Semester;
            section.Year = dto.Year;
            section.AvailableSeats = dto.AvailableSeats;
            section.InstructorId = dto.InstructorId;

            sectionRepository.Update(section);
            await unitOfWork.SaveChangesAsync();

            return await GetSectionByIdAsync(section.SectionId);
        }

        // =========================
        // Delete
        // =========================
        public async Task<bool> DeleteSectionAsync(int id)
        {
            var deleted = await sectionRepository.DeleteAsync(id);
            if (!deleted)
                throw new Exception("Section not found");

            await unitOfWork.SaveChangesAsync();
            return true;
        }

        // =========================
        // Shared Query
        // =========================
        private IQueryable<ViewSectionDTO> BuildBaseQuery()
        {
            return sectionRepository
                .GetAllAsQueryable()
                .Include(s => s.Course)
                .Include(s => s.Instructor)
                    .ThenInclude(i => i.User)
                .Include(s => s.Slots)
                    .ThenInclude(sl => sl.Room)
                .Include(s => s.Slots)
                    .ThenInclude(sl => sl.TimeSlot)
                .Include(s => s.Slots)
                    .ThenInclude(sl => sl.Instructor)
                        .ThenInclude(i => i.User)
                .Select(s => new ViewSectionDTO
                {
                    SectionId = s.SectionId,
                    Semester = s.Semester,
                    Year = s.Year,
                    InstructorId = s.InstructorId,
                    InstructorName = s.Instructor != null
                        ? s.Instructor.User.FullName
                        : null,
                    AvailableSeats = s.AvailableSeats,

                    CourseSummary = new ViewCourseSummaryDTO
                    {
                        CourseId = s.Course.CourseId,
                        CourseName = s.Course.CourseName,
                        CourseCode = s.Course.CourseCode,
                        CreditHours = s.Course.CreditHours
                    },

                    ScheduleSlots = s.Slots.Select(slot => new ViewScheduleSlotDTO
                    {
                        ScheduleSlotId = slot.ScheduleSlotId,
                        SectionId = slot.SectionId,
                        SectionName =
                            s.Course.CourseName + " - Section " + s.SectionId,

                        RoomId = slot.RoomId,
                        Room =
                            slot.Room.Building + " - " + slot.Room.RoomNumber,

                        TimeSlotId = slot.TimeSlotId,
                        TimeSlot =
                            slot.TimeSlot.Day + " " +
                            slot.TimeSlot.StartTime + "-" +
                            slot.TimeSlot.EndTime,

                        InstructorId = slot.InstructorId,
                        InstructorName = slot.Instructor.User.FullName,
                        SlotType = slot.SlotType.ToString()
                    }).ToList()
                });
        }


        // =========================
        // Mapper
        // =========================
        private static ViewSectionDTO MapToViewDTO(Section s)
        {
            return new ViewSectionDTO
            {
                SectionId = s.SectionId,
                Semester = s.Semester,
                Year = s.Year,
                InstructorId = s.InstructorId,
                InstructorName = s.Instructor != null ? s.Instructor.User.FullName : null,
                AvailableSeats = s.AvailableSeats,

                CourseSummary = new ViewCourseSummaryDTO
                {
                    CourseId = s.Course.CourseId,
                    CourseName = s.Course.CourseName,
                    CourseCode = s.Course.CourseCode,
                    CreditHours = s.Course.CreditHours
                },

                ScheduleSlots = s.Slots.Select(slot => new ViewScheduleSlotDTO
                {
                    ScheduleSlotId = slot.ScheduleSlotId,
                    SectionId = slot.SectionId,
                    SectionName = s.Course.CourseName + " - Section " + s.SectionId,
                    RoomId = slot.RoomId,
                    Room = slot.Room.Building + " - " + slot.Room.RoomNumber,
                    TimeSlotId = slot.TimeSlotId,
                    TimeSlot = slot.TimeSlot.Day + " " +
                               slot.TimeSlot.StartTime + "-" +
                               slot.TimeSlot.EndTime,
                    InstructorId = slot.InstructorId,
                    InstructorName = slot.Instructor.User.FullName,
                    SlotType = slot.SlotType.ToString()
                }).ToList()
            };
        }
    }
}
