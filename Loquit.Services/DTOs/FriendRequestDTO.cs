using Loquit.Data.Entities;
using Loquit.Services.DTOs.AbstracionsDTOs;

namespace Loquit.Services.DTOs
{
    public class FriendRequestDTO : BaseDTO
    {
        public string SentByUserId { get; set; }
        public virtual AppUser? SentByUser { get; set; }

        public string SentToUserId { get; set; }
        public virtual AppUser? SentToUser { get; set; }
    }
}
