using Loquit.Services.DTOs.AbstracionsDTOs;
using Loquit.Services.DTOs.ChatTypesDTOs;
using Loquit.Services.DTOs;

namespace Loquit.Web.Models
{
    public class DirectChatViewModel
    {
        public ChatsListViewModel ChatsList { get; set; }
        public CurrentChatViewModel CurrentChat { get; set; }
    }
}
