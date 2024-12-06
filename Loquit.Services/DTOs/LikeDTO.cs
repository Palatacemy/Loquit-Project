using Loquit.Data.Entities;
using Loquit.Services.DTOs.AbstracionsDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Services.DTOs
{
    public class LikeDTO : BaseDTO
    {
        public int PostId { get; set; }
        public virtual Post? Post { get; set; }
        public int CommentId { get; set; }
        public virtual Comment? Comment { get; set; }
        public string UserId { get; set; }
        public virtual AppUser? User { get; set; }
    }
}
