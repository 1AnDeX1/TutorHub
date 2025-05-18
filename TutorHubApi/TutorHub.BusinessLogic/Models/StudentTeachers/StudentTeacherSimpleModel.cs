using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorHub.DataAccess.Enums;

namespace TutorHub.BusinessLogic.Models.StudentTeachers
{
    public class StudentTeacherSimpleModel
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int TeacherId { get; set; }
        public StudentTeacherStatus Status { get; set; }
    }
}
