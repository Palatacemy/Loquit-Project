using AutoMapper;
using Loquit.Data.Entities.Abstractions;
using Loquit.Data.Entities.MessageTypes;
using Loquit.Services.DTOs.AbstracionsDTOs;
using Loquit.Services.DTOs.MessageTypesDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Services.Profiles
{
    public class MessagesProfile : Profile
    {
        public MessagesProfile()
        {
            CreateMap<BaseMessage, BaseMessageDTO>()
                .Include<TextMessage, TextMessageDTO>()
                .Include<ImageMessage, ImageMessageDTO>()
                .ReverseMap();
        }
    }
}
