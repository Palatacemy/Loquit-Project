using System;
using Loquit.Data.Entities;
using Loquit.Data.Entities.ChatTypes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Loquit.Services.DTOs.ChatTypesDTOs;

namespace Loquit.Services.Profiles.ChatTypesProfiles
{
    public class GroupChatProfile : Profile
    {
        public GroupChatProfile()
        {
            CreateMap<GroupChat, GroupChatDTO>()
                .ReverseMap();
        }
    }
}
