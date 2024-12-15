using Loquit.Data.Entities;
using Loquit.Data.Entities.ChatTypes;
using Loquit.Services.DTOs.AbstracionsDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Services.DTOs
{
    public class InviteDTO : BaseDTO
    {
        public int PostId { get; set; }
        public virtual GroupChat? Chat { get; set; }
        public string UserId { get; set; }
        public virtual AppUser? User { get; set; }
    }
}
