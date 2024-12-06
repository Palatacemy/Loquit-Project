using Loquit.Data.Entities;
using Loquit.Services.DTOs.AbstracionsDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Services.DTOs
{
    public class CommentDTO : BaseDTO
    {
        public CommentDTO()
        {
            IsEdited = false;
            Likes = 0;
            Dislikes = 0;
            RepliesCount = 0;
    }
        public string Text { get; set; }
        public virtual AppUser? Commenter { get; set; }
        public DateTime TimeOfCommenting { get; set; }
        public bool IsEdited { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public virtual List<Like>? LikedBy { get; set; }
        public virtual List<Dislike>? DislikedBy { get; set; }
        public int RepliesCount { get; set; }
        public virtual Post? Post { get; set; }
        public int PostId { get; set; }
        public virtual Comment? Parent { get; set; }
        public int? ParentId { get; set; }
        public List<Comment>? Replies { get; set; }
    }
}
