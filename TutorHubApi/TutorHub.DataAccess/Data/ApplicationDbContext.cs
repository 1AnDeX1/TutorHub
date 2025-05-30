using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TutorHub.DataAccess.Entities;

namespace TutorHub.DataAccess.Data;

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

    public DbSet<Chat> Chats { get; set; }

    public DbSet<ChatMessage> ChatMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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

        var timeOnlyConverter = new ValueConverter<TimeOnly, TimeSpan>(
            v => v.ToTimeSpan(),
            v => TimeOnly.FromTimeSpan(v));

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.Property(e => e.StartTime)
                .HasConversion(timeOnlyConverter);

            entity.Property(e => e.EndTime)
                  .HasConversion(timeOnlyConverter);

            entity.HasOne(s => s.StudentTeacher)
              .WithMany(st => st.Schedules)
              .HasForeignKey(s => s.StudentTeacherId)
              .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TeacherAvailability>(entity =>
        {
            entity.Property(e => e.StartTime)
                .HasConversion(timeOnlyConverter);

            entity.Property(e => e.EndTime)
                  .HasConversion(timeOnlyConverter);
        });

        modelBuilder.Entity<TeacherRating>()
            .HasOne(tr => tr.Teacher)
            .WithMany()
            .HasForeignKey(tr => tr.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Chat>()
            .HasOne(c => c.Teacher)
            .WithMany()
            .HasForeignKey(c => c.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Chat>()
            .HasOne(c => c.Student)
            .WithMany()
            .HasForeignKey(c => c.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ChatMessage>()
            .HasOne(m => m.Chat)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.ChatId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
