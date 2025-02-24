using Loquit.Data.Entities.Abstractions;

namespace Loquit.Data.Entities
{
    public class FriendRequest : BaseEntity
    {
        //represents a friend request
        
        public string SentByUserId { get; set; }
        public virtual AppUser? SentByUser { get; set; }

        public string SentToUserId { get; set; }
        public virtual AppUser? SentToUser { get; set; }
    }
}
