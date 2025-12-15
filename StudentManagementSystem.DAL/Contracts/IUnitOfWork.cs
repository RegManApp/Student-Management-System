using StudentManagementSystem.DAL.Entities;

namespace StudentManagementSystem.DAL.Contracts
{
    public interface IUnitOfWork
    {
        IBaseRepository<Course> Courses { get; }
        IBaseRepository<Room> Rooms { get; }
        IBaseRepository<TimeSlot> TimeSlots { get; }
        IBaseRepository<Section> Sections { get; }
        IBaseRepository<ScheduleSlot> ScheduleSlots { get; }
        IBaseRepository<Enrollment> Enrollments { get; }

        IBaseRepository<AdminProfile> AdminProfiles { get; }
        IBaseRepository<InstructorProfile> InstructorProfiles { get; }
        IBaseRepository<StudentProfile> StudentProfiles { get; }
        IBaseRepository<AcademicPlan> AcademicPlans { get; }

        public IBaseRepository<Course> Courses { get; }
        public IBaseRepository<Cart> Carts { get; }
        public IBaseRepository<CartItem> CartItems { get; }
        public IBaseRepository<Room> Rooms { get; }
        public IBaseRepository<TimeSlot> TimeSlots { get; }
        public IBaseRepository<Section> Sections { get; }
        public IBaseRepository<ScheduleSlot> ScheduleSlots { get; }
        //public IBaseRepository<Transcript> Transcripts { get; }
        public IBaseRepository<Enrollment> Enrollments { get; }
        public IBaseRepository<AdminProfile> AdminProfiles { get; }
        public IBaseRepository<InstructorProfile> InstructorProfiles { get; }
        public IBaseRepository<StudentProfile> StudentProfiles { get; }
        public IBaseRepository<AcademicPlan> AcademicPlans { get; }
        Task SaveChangesAsync();
    }
}
