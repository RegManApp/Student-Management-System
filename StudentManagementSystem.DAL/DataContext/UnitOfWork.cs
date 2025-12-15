using StudentManagementSystem.DAL.Contracts;
using StudentManagementSystem.DAL.Entities;
using StudentManagementSystem.DAL.Repositories;

namespace StudentManagementSystem.DAL.DataContext
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext dbcontext;

        private readonly Lazy<IBaseRepository<Course>> courses;
        private readonly Lazy<IBaseRepository<Room>> rooms;
        private readonly Lazy<IBaseRepository<TimeSlot>> timeSlots;
        private readonly Lazy<IBaseRepository<Section>> sections;
        private readonly Lazy<IBaseRepository<ScheduleSlot>> scheduleSlots;
        private readonly Lazy<IBaseRepository<Enrollment>> enrollments;

        private readonly Lazy<IBaseRepository<AdminProfile>> adminProfiles;
        private readonly Lazy<IBaseRepository<InstructorProfile>> instructorProfiles;
        private readonly Lazy<IBaseRepository<StudentProfile>> studentProfiles;
        private readonly Lazy<IBaseRepository<AcademicPlan>> academicPlans;

        public UnitOfWork(AppDbContext context)
        {
            dbcontext = context;

            courses = new Lazy<IBaseRepository<Course>>(
                () => new BaseRepository<Course>(dbcontext));

            rooms = new Lazy<IBaseRepository<Room>>(
                () => new BaseRepository<Room>(dbcontext));

            timeSlots = new Lazy<IBaseRepository<TimeSlot>>(
                () => new BaseRepository<TimeSlot>(dbcontext));

            sections = new Lazy<IBaseRepository<Section>>(
                () => new BaseRepository<Section>(dbcontext));

            scheduleSlots = new Lazy<IBaseRepository<ScheduleSlot>>(
                () => new BaseRepository<ScheduleSlot>(dbcontext));

            enrollments = new Lazy<IBaseRepository<Enrollment>>(
                () => new BaseRepository<Enrollment>(dbcontext));

            adminProfiles = new Lazy<IBaseRepository<AdminProfile>>(
                () => new BaseRepository<AdminProfile>(dbcontext));

            instructorProfiles = new Lazy<IBaseRepository<InstructorProfile>>(
                () => new BaseRepository<InstructorProfile>(dbcontext));

            studentProfiles = new Lazy<IBaseRepository<StudentProfile>>(
                () => new BaseRepository<StudentProfile>(dbcontext));

            academicPlans = new Lazy<IBaseRepository<AcademicPlan>>(
                () => new BaseRepository<AcademicPlan>(dbcontext));
        }

        public IBaseRepository<Course> Courses => courses.Value;
        public IBaseRepository<Room> Rooms => rooms.Value;
        public IBaseRepository<TimeSlot> TimeSlots => timeSlots.Value;
        public IBaseRepository<Section> Sections => sections.Value;
        public IBaseRepository<ScheduleSlot> ScheduleSlots => scheduleSlots.Value;
        public IBaseRepository<Enrollment> Enrollments => enrollments.Value;

        public IBaseRepository<AdminProfile> AdminProfiles => adminProfiles.Value;
        public IBaseRepository<InstructorProfile> InstructorProfiles => instructorProfiles.Value;
        public IBaseRepository<StudentProfile> StudentProfiles => studentProfiles.Value;
        public IBaseRepository<AcademicPlan> AcademicPlans => academicPlans.Value;

        public async Task SaveChangesAsync()
        {
            await dbcontext.SaveChangesAsync();
        }
    }
}
