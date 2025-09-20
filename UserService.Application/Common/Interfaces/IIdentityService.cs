using UserService.Application.Models;
using UserService.Domain.Entities;

namespace UserService.Application.Common.Interfaces;

public interface IIdentityService
{
    public Task<UserModel> SignInAsync(SignInModel model);
    public Task<UserModel> SignUpAsync(SignUpModel model);
}