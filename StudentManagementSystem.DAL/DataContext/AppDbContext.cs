using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.DAL.Entities;

namespace StudentManagementSystem.DAL.DataContext
{
    public class AppDbContext : IdentityDbContext<BaseUser>
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Section> Sections { get; set; }

        // public DbSet<BaseUser> Users { get; set; }

        public DbSet<AdminProfile> Admins { get; set; }
        public DbSet<StudentProfile> Students { get; set; }
        public DbSet<InstructorProfile> Instructors { get; set; }

        public DbSet<ScheduleSlot> ScheduleSlots { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<AcademicPlan> AcademicPlans { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ============================
            // 1. ONE-TO-ONE RELATIONSHIPS
            // ============================

            modelBuilder.Entity<StudentProfile>()
                .HasOne(sp => sp.User)
                .WithOne(u => u.StudentProfile)
                .HasForeignKey<StudentProfile>(sp => sp.UserId);

            modelBuilder.Entity<AdminProfile>()
                .HasOne(ap => ap.User)
                .WithOne(u => u.AdminProfile)
                .HasForeignKey<AdminProfile>(ap => ap.UserId);

            modelBuilder.Entity<InstructorProfile>()
                .HasOne(ip => ip.User)
                .WithOne(u => u.InstructorProfile)
                .HasForeignKey<InstructorProfile>(ip => ip.UserId);

            modelBuilder.Entity<StudentProfile>()
                .HasOne(sp => sp.AcademicPlan)
                .WithOne(ap => ap.Student)
                .HasForeignKey<StudentProfile>(sp => sp.AcademicPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            // ============================
            // 2. ONE-TO-MANY RELATIONSHIPS
            // ============================

            // Section → ScheduleSlot
            modelBuilder.Entity<ScheduleSlot>()
                .HasOne(ss => ss.Section)
                .WithMany(s => s.Slots)
                .HasForeignKey(ss => ss.SectionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Room → ScheduleSlot
            modelBuilder.Entity<ScheduleSlot>()
                .HasOne(ss => ss.Room)
                .WithMany(r => r.ScheduleSlots)
                .HasForeignKey(ss => ss.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            // TimeSlot → ScheduleSlot
            modelBuilder.Entity<ScheduleSlot>()
                .HasOne(ss => ss.TimeSlot)
                .WithMany()
                .HasForeignKey(ss => ss.TimeSlotId)
                .OnDelete(DeleteBehavior.Restrict);

            // Instructor → ScheduleSlot  ✅ (Feature 11)
            modelBuilder.Entity<ScheduleSlot>()
                .HasOne(ss => ss.Instructor)
                .WithMany()
                .HasForeignKey(ss => ss.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Section → Enrollment
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Section)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.SectionId)
                .OnDelete(DeleteBehavior.Restrict);

            // StudentProfile → Enrollment
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            // ============================
            // 3. UNIQUE CONSTRAINTS
            // ============================

            modelBuilder.Entity<Enrollment>()
                .HasIndex(e => new { e.StudentId, e.SectionId })
                .IsUnique();

            // ============================
            // 4. ENUM CONVERSIONS
            // ============================

            modelBuilder.Entity<Course>()
                .Property(c => c.CourseCategory)
                .HasConversion<string>();

            modelBuilder.Entity<Enrollment>()
                .Property(e => e.Status)
                .HasConversion<string>();

            modelBuilder.Entity<TimeSlot>()
                .Property(t => t.Day)
                .HasConversion<string>();
        }
    }
}
