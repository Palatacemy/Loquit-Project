using Loquit.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Data.Entities
{
    public class Save : BaseEntity
    {
        //represents a single save of a post
        public int PostId { get; set; }
        public virtual Post? Post { get; set; }
        public string UserId { get; set; }
        public virtual AppUser? User { get; set; }
    }
}
