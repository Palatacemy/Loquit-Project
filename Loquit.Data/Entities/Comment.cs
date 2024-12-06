using Loquit.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Data.Entities
{
    public class Comment : BaseEntity
    {
        //a comment to a post
        public Comment()
        {
            LikedBy = new HashSet<Like>();
            DislikedBy = new HashSet<Dislike>();
            Replies = new HashSet<Comment>();
            IsEdited = false;
            Likes = 0;
            Dislikes = 0;
        }
        public string Text { get; set; }
        public virtual AppUser? Commenter { get; set; }
        public DateTime TimeOfCommenting { get; set; }
        public bool IsEdited { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public virtual ICollection<Like>? LikedBy { get; set; }
        public virtual ICollection<Dislike>? DislikedBy { get; set; }
        public virtual Post? Post { get; set; }
        public int PostId { get; set; }
        public virtual Comment? Parent { get; set; }
        public int? ParentId { get; set; }
        public virtual ICollection<Comment>? Replies { get; set; }
    }
}
