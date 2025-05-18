namespace TutorHub.DataAccess.Entities
{
    public class Student
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public int Age { get; set; }

        public int? Grade {  get; set; }

        public string? Description { get; set; }

        public User? User { get; set; }

        public ICollection<StudentTeacher>? Teachers { get; set; }

        public ICollection<Schedule>? Lessons { get; set; }
    }
}
