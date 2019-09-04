using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Security.Core.Entities;

namespace Security.Core.Services
{
    public interface IAccountManager
    {
        Task<bool> CheckPasswordAsync(User user, string password);

        Task<User> GetUserByEmailAsync(string email);

        Task<User> GetUserByIdAsync(Guid userId);

        Task<User> GetUserByUserNameAsync(string userName);

        Task<bool> ResetPasswordAsync(User user, string newPassword);

        Task<bool> UpdatePasswordAsync(User user, string currentPassword, string newPassword);

        Task<bool> UpdateUserAsync(User user);

        Task<bool> UpdateUserAsync(User user, IEnumerable<string> roles);
    }
}
