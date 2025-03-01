using Loquit.Services.DTOs;
using Loquit.Services.DTOs.AbstracionsDTOs;

namespace Loquit.Web.Models
{
    public class MessageViewModel
    {
        public BaseMessageDTO Message { get; set; }
        public ChatParticipantUserDTO SenderUser { get; set; }
        public BaseMessageDTO? PreviousMessage { get; set; }
    }
}
