﻿using AutoMapper;
using Loquit.Data.Entities;
using Loquit.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Services.Profiles
{
    public class ChatUserProfile : Profile
    {
        public ChatUserProfile()
        {
            CreateMap<ChatUser, ChatUserDTO>()
                .ReverseMap();
        }
    }
}
