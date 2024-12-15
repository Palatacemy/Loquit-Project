using Loquit.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
