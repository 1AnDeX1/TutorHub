using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorHub.BusinessLogic.Models.User.Student
{
    public class StudentModel
    {
        public int Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public int Age { get; set; }

        public int? Grade { get; set; }

        public string? Description { get; set; }
    }
}
