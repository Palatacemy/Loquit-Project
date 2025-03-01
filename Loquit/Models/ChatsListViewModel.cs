using Loquit.Services.DTOs.ChatTypesDTOs;

namespace Loquit.Web.Models
{
    public class ChatsListViewModel
    {
        public IEnumerable<DirectChatDTO> DirectChats { get; set; }
        //public IEnumerable<GroupChatDTO> GroupChats { get; set; } //for future implementation
    }
}
