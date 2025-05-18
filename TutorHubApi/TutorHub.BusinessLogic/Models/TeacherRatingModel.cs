using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorHub.BusinessLogic.Models
{
    public class TeacherRatingModel
    {
        public int StudentId { get; set; }

        public int TeacherId { get; set; }

        public int Value { get; set; }
    }
}
