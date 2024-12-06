using Loquit.Services.DTOs.ChatTypesDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Services.Abstractions.ChatTypesAbstractions
{
    public interface IDirectChatService
    {
        Task<List<DirectChatDTO>> GetDirectChatsAsync();
        Task<DirectChatDTO> GetDirectChatByIdAsync(int id);
        Task AddDirectChatAsync(DirectChatDTO directChat);
        Task DeleteDirectChatByIdAsync(int id);
        Task UpdateDirectChatAsync(DirectChatDTO directChat);
    }
}
