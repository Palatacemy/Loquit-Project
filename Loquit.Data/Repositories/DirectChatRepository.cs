using Loquit.Data.Entities.Abstractions;
using Loquit.Data.Entities.ChatTypes;
using Loquit.Data.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Data.Repositories
{
    public class DirectChatRepository : CrudRepository<DirectChat>, IDirectChatRepository
    {
        private readonly ApplicationDbContext _context;
        public DirectChatRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<DirectChat?> GetDirectChatWithMessagesAsync(int chatId)
        {
            return await _context.DirectChats
                .Include(c => c.Messages)
                .Include(c => c.Members)
                .FirstOrDefaultAsync(c => c.Id == chatId);
        }

        public async Task<List<BaseMessage>> GetMessagesByChatIdAsync(int chatId)
        {
            var textMessages = await _context.TextMessages
                .Where(m => m.ChatId == chatId)
                .ToListAsync();

            var imageMessages = await _context.ImageMessages
                .Where(m => m.ChatId == chatId)
                .ToListAsync();

            var messages = new List<BaseMessage>();
            messages.AddRange(textMessages);
            messages.AddRange(imageMessages);

            return messages;
        }

        public async Task<List<BaseMessage>> GetMessagesBeforeIdAsync(int chatId, int lastMessageId, int count)
        {
            var textMessages = await _context.TextMessages
                .Where(m => m.ChatId == chatId && m.Id < lastMessageId)
                .OrderByDescending(m => m.TimeOfSending)
                .Take(count)
                .ToListAsync();

            var imageMessages = await _context.ImageMessages
                .Where(m => m.ChatId == chatId && m.Id < lastMessageId)
                .OrderByDescending(m => m.TimeOfSending)
                .Take(count)
                .ToListAsync();

            return textMessages.Cast<BaseMessage>()
                .Concat(imageMessages)
                .OrderByDescending(m => m.TimeOfSending)
                .Take(count)
                .ToList();
        }
    }
}
