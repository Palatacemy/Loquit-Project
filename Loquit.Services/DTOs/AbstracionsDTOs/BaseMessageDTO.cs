using Loquit.Data.Entities.Abstractions;
using Loquit.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Services.DTOs.AbstracionsDTOs
{
    public abstract class BaseMessageDTO : BaseDTO
    {
        public DateTime TimeOfSending { get; set; }
        public string SenderUserId { get; set; }
        public virtual AppUser? SenderUser { get; set; }
        /*public virtual BaseMessage? Parent { get; set; }
        public int ParentId { get; set; }
        public List<BaseMessage> Replies { get; set; }*/
    }
}
