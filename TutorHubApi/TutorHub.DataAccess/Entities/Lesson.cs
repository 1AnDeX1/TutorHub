namespace TutorHub.DataAccess.Entities
{
    public class Lesson
    {
        public int Id { get; set; }

        public int LessonId { get; set; }

        public DateOnly Date { get; set; }

        public string? Notes { get; set; }

        public Schedule Schedule { get; set; }
    }
}
