using Loquit.Data.Entities;
using Loquit.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Data.Repositories.Abstractions
{
    public interface IChatUserRepository
    {
        Task AddUserToChatAsync(ChatUser chatUser);
        Task RemoveUserFromChatAsync(string userId, int chatId);
        Task<List<ChatUser>> GetUsersInChatAsync(int chatId);
        Task<List<ChatUser>> GetChatsForUserAsync(string userId);
        Task<ChatUser?> GetChatUserAsync(string userId, int chatId);
        Task<int> CountUsersInChatAsync(int chatId);
    }
}
