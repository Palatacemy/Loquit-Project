using Loquit.Services.DTOs.MessageTypesDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Services.Abstractions.MessageTypesAbstractions
{
    public interface ITextMessageService
    {
        Task<List<TextMessageDTO>> GetTextMessagesAsync();
        Task<TextMessageDTO> GetTextMessageByIdAsync(int id);
        Task AddTextMessageAsync(TextMessageDTO textMessage);
        Task DeleteTextMessageByIdAsync(int id);
        Task UpdateTextMessageAsync(TextMessageDTO textMessage);
    }
}
