using Loquit.Data.Entities;
using Loquit.Services.DTOs;
using Loquit.Services.DTOs.AbstracionsDTOs;
using Loquit.Services.DTOs.ChatTypesDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Services.Services.Abstractions.ChatTypesAbstractions
{
    public interface IDirectChatService
    {
        Task<List<DirectChatDTO>> GetDirectChatsAsync();
        Task<DirectChatDTO> GetDirectChatByIdAsync(int id);
        Task AddDirectChatAsync(DirectChatDTO directChat);
        Task DeleteDirectChatByIdAsync(int id);
        Task UpdateDirectChatAsync(DirectChatDTO directChat);
        Task<List<ChatParticipantUserDTO?>> GetUsersInChatAsync(int chatId);
        Task<List<DirectChatDTO?>> GetChatsForUserAsync(string userId);
        Task<DirectChatDTO> AddMessageToChatAsync(DirectChatDTO directChatDTO, BaseMessageDTO messageDTO);
        Task<DirectChatDTO?> GetDirectChatWithMessagesAsync(int chatId);
        Task<List<BaseMessageDTO>> GetMessagesForChatAsync(int chatId);
        Task<List<BaseMessageDTO>> GetMessagesBeforeIdAsync(int chatId, int lastMessageId, int count);
    }
}
