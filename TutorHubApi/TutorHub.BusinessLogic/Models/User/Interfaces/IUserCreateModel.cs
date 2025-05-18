using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorHub.BusinessLogic.Models.User.Interfaces
{
    public interface IUserCreateModel
    {
        string UserName { get; set; }
        string Email { get; set; }
        string? Password { get; set; }
    }
}
