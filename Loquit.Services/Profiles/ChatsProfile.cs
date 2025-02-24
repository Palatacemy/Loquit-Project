using AutoMapper;
using Loquit.Data.Entities.Abstractions;
using Loquit.Data.Entities.ChatTypes;
using Loquit.Services.DTOs.AbstracionsDTOs;
using Loquit.Services.DTOs.ChatTypesDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Services.Profiles
{
    public class ChatsProfile : Profile
    {
        public ChatsProfile()
        {
            CreateMap<BaseChat, BaseChatDTO>()
                .Include<DirectChat, DirectChatDTO>()
                .Include<GroupChat, GroupChatDTO>()
                .ReverseMap();
        }
    }
}
