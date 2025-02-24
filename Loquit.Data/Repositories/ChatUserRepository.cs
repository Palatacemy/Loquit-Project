using Loquit.Data.Entities.Abstractions;
using Loquit.Data.Entities;
using Loquit.Data.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Data.Repositories
{
    public class ChatUserRepository : IChatUserRepository
    {
        private readonly ApplicationDbContext _context;

        public ChatUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddUserToChatAsync(ChatUser chatUser)
        {
            bool chatUserExists = await _context.ChatUsers
                .AnyAsync(cu => cu.ChatId == chatUser.ChatId && cu.UserId == chatUser.UserId);

            if (!chatUserExists)
            {
                await _context.ChatUsers.AddAsync(chatUser);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveUserFromChatAsync(string userId, int chatId)
        {
            var chatUser = await _context.ChatUsers
                .FirstOrDefaultAsync(cu => cu.UserId == userId && cu.ChatId == chatId);
            if (chatUser != null)
            {
                _context.ChatUsers.Remove(chatUser);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ChatUser>> GetUsersInChatAsync(int chatId)
        {
            return await _context.ChatUsers
                .Where(cu => cu.ChatId == chatId)
                .Include(cu => cu.User)
                .ToListAsync();
        }

        public async Task<List<ChatUser>> GetChatsForUserAsync(string userId)
        {
            return await _context.ChatUsers
                .Where(cu => cu.UserId == userId)
                .Include(cu => cu.Chat)
                .ToListAsync();
        }

        public async Task<ChatUser?> GetChatUserAsync(string userId, int chatId)
        {
            return await _context.ChatUsers
                .FirstOrDefaultAsync(cu => cu.UserId == userId && cu.ChatId == chatId);
        }

        public async Task<int> CountUsersInChatAsync(int chatId)
        {
            return await _context.ChatUsers.CountAsync(cu => cu.ChatId == chatId);
        }
    }
}
