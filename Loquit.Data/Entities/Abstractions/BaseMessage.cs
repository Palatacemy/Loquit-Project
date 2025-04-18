﻿using Loquit.Data.Entities.ChatTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Data.Entities.Abstractions
{
    public abstract class BaseMessage : BaseEntity
    {
        public BaseMessage()
        {
            IsRead = false;
        }
        //abstraction for all types of messages in direct chats
        public DateTime TimeOfSending { get; set; }
        public string SenderUserId { get; set; }
        public virtual AppUser? SenderUser { get; set; }
        public int ChatId { get; set; }
        public virtual BaseChat? Chat { get; set; }
        public bool IsRead { get; set; }

        //not implemented reply functionality for messages:

        /*public virtual BaseMessage? Parent { get; set; }
        public int ParentId { get; set; }
        public ICollection<BaseMessage>? Replies { get; set; }*/
    }
}