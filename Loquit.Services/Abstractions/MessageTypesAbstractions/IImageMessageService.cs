using Loquit.Services.DTOs.MessageTypesDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Services.Abstractions.MessageTypesAbstractions
{
    public interface IImageMessageService
    {
        Task<List<ImageMessageDTO>> GetImageMessagesAsync();
        Task<ImageMessageDTO> GetImageMessageByIdAsync(int id);
        Task AddImageMessageAsync(ImageMessageDTO imageMessage);
        Task DeleteImageMessageByIdAsync(int id);
        Task UpdateImageMessageAsync(ImageMessageDTO imageMessage);
    }
}
