using Loquit.Services.DTOs;
using Loquit.Services.DTOs.AbstracionsDTOs;
using Loquit.Services.DTOs.ChatTypesDTOs;

namespace Loquit.Web.Models
{
    public class CurrentChatViewModel
    {
        public BaseChatDTO CurrentChat { get; set; }
        public List<BaseMessageDTO> Messages { get; set; }
        public ChatParticipantUserDTO CurrentUser { get; set; }
    }
}
