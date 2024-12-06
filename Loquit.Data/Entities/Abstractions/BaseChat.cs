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
        public virtual List<AppUser> Members { get; set; }
        public virtual List<BaseMessage>? Messages { get; set; }
    }
}