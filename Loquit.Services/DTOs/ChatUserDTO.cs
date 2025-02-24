using Loquit.Data.Entities.Abstractions;
using Loquit.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loquit.Services.DTOs.AbstracionsDTOs;

namespace Loquit.Services.DTOs
{
    public class ChatUserDTO
    {
        public int ChatId { get; set; }
        public BaseChatDTO? Chat { get; set; }
        public string UserId { get; set; }
        public ChatParticipantUserDTO? User { get; set; }
        public DateTime TimeOfJoining { get; set; }
    }
}
