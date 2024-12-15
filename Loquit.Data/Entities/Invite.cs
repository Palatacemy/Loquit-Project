using Loquit.Data.Entities.Abstractions;
using Loquit.Data.Entities.ChatTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Data.Entities
{
    public class Invite : BaseEntity
    {
        //represents an invite to a group chat
        public int ChatId { get; set; }
        public virtual GroupChat? Chat { get; set; }
        public string UserId { get; set; }
        public virtual AppUser? User { get; set; }
    }
}
