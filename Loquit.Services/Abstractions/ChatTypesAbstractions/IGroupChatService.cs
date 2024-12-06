using Loquit.Services.DTOs.ChatTypesDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Services.Abstractions.ChatTypesAbstractions
{
    public interface IGroupChatService
    {
        Task<List<GroupChatDTO>> GetGroupChatsAsync();
        Task<GroupChatDTO> GetGroupChatByIdAsync(int id);
        Task AddGroupChatAsync(GroupChatDTO groupChat);
        Task DeleteGroupChatByIdAsync(int id);
        Task UpdateGroupChatAsync(GroupChatDTO groupChat);
    }
}
