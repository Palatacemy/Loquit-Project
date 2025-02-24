using Loquit.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Data.Entities
{
    public class Post : BaseEntity
    {
        //a public post, can be commented on
        public Post()
        {
            Title = "";
            BodyText = "";
            Comments = new HashSet<Comment>();
            LikedBy = new HashSet<Like>();
            DislikedBy = new HashSet<Dislike>();
            SavedBy = new HashSet<Save>();
            CategoryId = 0;
            Evaluations = Evaluate(Title, BodyText);
            IsEdited = false;
            IsNsfw = false;
            IsSpoiler = false;
            Likes = 0;
            Dislikes = 0;
        }
        public string Title { get; set; }
        public string BodyText { get; set; }
        public string? PictureUrl { get; set; }
        public string CreatorId { get; set; }
        public virtual AppUser? Creator { get; set; }
        public DateTime TimeOfPosting { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Like>? LikedBy { get; set; }
        public virtual ICollection<Save>? SavedBy { get; set; }
        public virtual ICollection<Dislike>? DislikedBy { get; set; }
        public int CategoryId { get; set; }
        public double[] Evaluations { get; set; }
        public bool IsSpoiler { get; set; }
        public bool IsNsfw { get; set; }
        public bool IsEdited { get; set; }
        public double[] Evaluate(string title, string bodyText)
        {
            string[] eval1 = { "0", "0" };
            string[] eval2 = { "0", "0" };
            string[] eval3 = { "0", "0" };
            string[] eval4 = { "0", "0" };
            string[] eval5 = { "0", "0" };
            int[] evals = { 1, 1, 1, 1, 1 };
            string[] titleWords = title.Split(' ', StringSplitOptions.TrimEntries).ToArray();
            string[] words = bodyText.Split(' ', StringSplitOptions.TrimEntries).ToArray();
            double total = 0;
            foreach (string word in titleWords)
            {
                if (eval1.Contains(word)) { evals[0] += 3; }
                if (eval2.Contains(word)) { evals[1] += 3; }
                if (eval3.Contains(word)) { evals[2] += 3; }
                if (eval4.Contains(word)) { evals[3] += 3; }
                if (eval5.Contains(word)) { evals[4] += 3; }
            }
            foreach (string word in words)
            {
                if (eval1.Contains(word)) { evals[0] += 1; }
                if (eval2.Contains(word)) { evals[1] += 1; }
                if (eval3.Contains(word)) { evals[2] += 1; }
                if (eval4.Contains(word)) { evals[3] += 1; }
                if (eval5.Contains(word)) { evals[4] += 1; }
            }
            total = evals.Sum();
            return [evals[0]/total, evals[1] / total, evals[2] / total, evals[3] / total, evals[4] / total];
        }
    }
}
