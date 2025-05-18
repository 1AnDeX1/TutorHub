using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TutorHub.BusinessLogic.Models.User.Interfaces;

namespace TutorHub.BusinessLogic.Models.User.Student
{
    public class StudentCreateModel : IUserCreateModel
    {
        public required string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address format")]
        public required string Email { get; set; }

        public required string Password { get; set; }

        public int Age { get; set; }

        public int? Grade { get; set; }

        public string? Description { get; set; }
    }
}
