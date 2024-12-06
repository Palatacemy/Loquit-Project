using AutoMapper;
using Loquit.Data.Entities.MessageTypes;
using Loquit.Services.DTOs.MessageTypesDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Services.Profiles.MessageTypesProfiles
{
    public class ImageMessageProfile : Profile
    {
        public ImageMessageProfile()
        {
            CreateMap<ImageMessage, ImageMessageDTO>()
                .ReverseMap();
        }
    }
}
