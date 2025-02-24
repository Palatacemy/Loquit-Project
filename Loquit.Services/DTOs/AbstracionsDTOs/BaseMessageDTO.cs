using Loquit.Data.Entities.Abstractions;
using Loquit.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Loquit.Services.DTOs.AbstracionsDTOs
{
    public abstract class BaseMessageDTO : BaseDTO
    {
        public BaseMessageDTO()
        {
            IsRead = false;
        }

        public DateTime TimeOfSending { get; set; }
        public string SenderUserId { get; set; }
        public virtual ChatParticipantUserDTO? SenderUser { get; set; }
        public int ChatId { get; set; }
        public virtual BaseChatDTO Chat { get; set; }
        public bool IsRead { get; set; }
        /*public virtual BaseMessage? Parent { get; set; }
        public int ParentId { get; set; }
        public List<BaseMessage> Replies { get; set; }*/
    }
}
