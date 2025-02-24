using Loquit.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Data.Entities
{
    public class ChatUser
    {
        public int ChatId { get; set; }
        public virtual BaseChat? Chat { get; set; }
        public string UserId { get; set; }
        public virtual AppUser? User { get; set; }
        public DateTime TimeOfJoining { get; set; }
    }
}
