using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TutorHub.DataAccess.Entities;

namespace TutorHub.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) { }

        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<StudentTeacher> StudentTeachers { get; set; }

        public DbSet<TeacherAvailability> TeacherAvailabilities { get; set; }

        public DbSet<Schedule> Schedules { get; set; }

        public DbSet<Lesson> LessonHistories { get; set; }

        public DbSet<TeacherRating> TeacherRatings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Teacher-Student Relationship (Many-to-Many through StudentTeacher)
            modelBuilder.Entity<StudentTeacher>(entity =>
            {
                entity.HasOne(st => st.Student)
                      .WithMany(s => s.Teachers)
                      .HasForeignKey(st => st.StudentId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(st => st.Teacher)
                      .WithMany(t => t.Students)
                      .HasForeignKey(st => st.TeacherId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Teacher-Lesson and Student-Lesson Relationships
            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasOne(l => l.StudentTeacher)
                      .WithMany(st => st.Schedules)
                      .HasForeignKey(l => l.StudentTeacherId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TeacherRating>()
                .HasOne(tr => tr.Teacher)
                .WithMany()
                .HasForeignKey(tr => tr.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
