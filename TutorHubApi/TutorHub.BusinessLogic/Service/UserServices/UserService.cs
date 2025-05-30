using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TutorHub.BusinessLogic.Exceptions;
using TutorHub.BusinessLogic.IServices.IUserServices;
using TutorHub.BusinessLogic.Models.User;
using TutorHub.BusinessLogic.Models.User.Interfaces;
using TutorHub.DataAccess.Entities;
using TutorHub.DataAccess.IRepositories;

namespace TutorHub.BusinessLogic.Service.UserServices;

public class UserService(
    IUnitOfWork unitOfWork,
    UserManager<User> userManager,
    IMapper mapper) : IUserService
{
    public async Task<UserModel?> GetUserByIdAsync(string id)
    {
        var user = await unitOfWork.Users.GetByIdAsync(id)
            ?? throw new NotFoundException($"User with ID {id} not found.");
        return mapper.Map<UserModel>(user);
    }

    public async Task UpdateUserAsync<T>(string id, T model) where T : IUserCreateModel
    {
        var user = await unitOfWork.Users.GetByIdAsync(id)
            ?? throw new NotFoundException($"User with ID {id} not found.");

        _ = mapper.Map(model, user);

        if (!string.IsNullOrEmpty(model?.Password))
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new ValidationException($"Failed to update password: {errors}");
            }
        }

        await unitOfWork.Users.UpdateAsync(user);
        await unitOfWork.SaveAsync();
    }

    public async Task DeleteUserAsync(string id)
    {
        _ = await unitOfWork.Users.GetByIdAsync(id)
            ?? throw new NotFoundException($"User with ID {id} not found.");

        await unitOfWork.Users.DeleteAsync(id);
    }
}
