using Loquit.Data.Entities.Abstractions;
using Loquit.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loquit.Services.DTOs.AbstracionsDTOs;

namespace Loquit.Services.DTOs
{
    public class FriendRequestDTO : BaseDTO
    {
        public string SentByUserId { get; set; }
        public virtual AppUser? SentByUser { get; set; }
        public string SentToUserId { get; set; }
        public virtual AppUser? SentToUser { get; set; }
    }
}
