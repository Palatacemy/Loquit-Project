using AutoMapper;
using Loquit.Data.Entities;
using Loquit.Data.Entities.ChatTypes;
using Loquit.Services.DTOs;
using Loquit.Services.DTOs.ChatTypesDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Services.Profiles.ChatTypesProfiles
{
    public class DirectChatProfile : Profile
    {
        public DirectChatProfile()
        {
            CreateMap<DirectChat, DirectChatDTO>()
                .ReverseMap();
        }
    }
}
