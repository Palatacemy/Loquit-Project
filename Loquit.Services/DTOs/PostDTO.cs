using Loquit.Data.Entities;
using Loquit.Services.DTOs.AbstracionsDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Services.DTOs
{
    public class PostDTO : BaseDTO
    {
        public PostDTO()
        {
            Evaluations = [0, 0, 0, 0, 0];
            IsEdited = false;
            IsNsfw = false;
            IsSpoiler = false;
            Likes = 0;
            Dislikes = 0;
            CommentsNumber = 0;
        }
        public string Title { get; set; }
        public string BodyText { get; set; }
        public string? PictureUrl { get; set; }
        public string CreatorId { get; set; }
        public virtual AppUser? Creator { get; set; }
        public DateTime TimeOfPosting { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public int CommentsNumber { get; set; }
        public virtual List<Comment>? Comments { get; set; }
        public virtual List<Like>? LikedBy { get; set; }
        public virtual List<Save>? SavedBy { get; set; }
        public virtual List<Dislike>? DislikedBy { get; set; }
        public int CategoryId { get; set; }
        public double[] Evaluations { get; set; }
        public bool IsSpoiler { get; set; }
        public bool IsNsfw { get; set; }
        public bool IsEdited { get; set; }
    }
}
