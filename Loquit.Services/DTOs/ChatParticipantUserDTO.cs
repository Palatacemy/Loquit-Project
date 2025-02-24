using Loquit.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Services.DTOs
{
    public class ChatParticipantUserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Description { get; set; }
        public virtual List<ChatUserDTO>? Chats { get; set; }
        public virtual List<ChatParticipantUserDTO>? Friends { get; set; }
    }
}
