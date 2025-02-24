using Loquit.Data.Entities;
using Loquit.Services.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Services.Services.Abstractions
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUserAsync(AppUser user, string password);
        Task<List<AppUser>> GetAllUsersAsync();
        Task<AppUser?> GetUserByIdAsync(string userId);
        Task<List<AppUser>> GetUserWithNameAsync(string username);
        Task<IdentityResult> UpdateUserAsync(AppUser user);
        Task<IdentityResult> DeleteUserAsync(string userId);
        Task<string> SendFriendRequest(string senderId, string recieverId);
        Task<string> DeclineFriendRequest(string senderId, string recieverId);
        Task<string> AcceptFriendRequest(string senderId, string recieverId);
        Task<string> RemoveFriend(string senderId, string recieverId);
        Task<string> CancelFriendRequest(string senderId, string recieverId);
        Task<ChatParticipantUserDTO?> GetChatParticipantUserDTOByIdAsync(string userId);
    }
}
