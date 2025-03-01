using Loquit.Data.Entities.ChatTypes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Data.Entities.Abstractions
{
    //abstraction for all direct chats
    public abstract class BaseChat : BaseEntity
    {
        protected BaseChat()
        {
            //Messages = new List<BaseMessage>();
            Members = new List<ChatUser>();
        }
        public virtual ICollection<ChatUser>? Members { get; set; }
        public virtual List<BaseMessage>? Messages { get; set; }
    }
}