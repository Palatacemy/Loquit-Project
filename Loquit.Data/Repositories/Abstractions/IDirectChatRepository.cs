using Loquit.Data.Entities.Abstractions;
using Loquit.Data.Entities.ChatTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Data.Repositories.Abstractions
{
    public interface IDirectChatRepository : ICrudRepository<DirectChat>
    {
        Task<DirectChat?> GetDirectChatWithMessagesAsync(int chatId);
        Task<List<BaseMessage>> GetMessagesByChatIdAsync(int chatId);
        Task<DirectChat?> GetDirectChatWithMessagesAndUsersAsync(int chatId);
        Task<List<BaseMessage>> GetMessagesBeforeIdAsync(int chatId, int lastMessageId, int count);
    }
}
