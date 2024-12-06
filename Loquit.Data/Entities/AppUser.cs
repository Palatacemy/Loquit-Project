using Loquit.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace Loquit.Data.Entities
{
    public class AppUser : IdentityUser
    {
        //user role, inheriting IdentityUser
        public AppUser()
        {
            Chats = new HashSet<BaseChat>();
            Posts = new HashSet<Post>();
            SavedPosts = new HashSet<Save>();
            LikedPosts = new HashSet<Like>();
            DislikedPosts = new HashSet<Dislike>();
            Friends = new HashSet<AppUser>();
            Blacklist = new HashSet<AppUser>();
            CategoryPreferences = [0.3, 0.3, 0.3, 0.3, 0.3, 0.3, 0.3, 0.3, 0.3];
            EvaluationPreferences = [0.3, 0.3, 0.3, 0.3, 0.3];
            AllowNsfw = false;
            ColorThemeId = 1;
            FriendRequestsSent = new HashSet<AppUser>();
            FriendRequestsReceived = new HashSet<AppUser>();
            ProfilePictureUrl = "user.png";
        }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Description { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public virtual ICollection<BaseChat>? Chats { get; set; }
        public virtual ICollection<Post>? Posts { get; set; }
        public virtual ICollection<Save>? SavedPosts { get; set; }
        public virtual ICollection<Like>? LikedPosts { get; set; }
        public virtual ICollection<Dislike>? DislikedPosts { get; set; }
        public virtual ICollection<AppUser>? Friends { get; set; }
        public virtual ICollection<AppUser>? Blacklist { get; set; }
        public double[] CategoryPreferences { get; set; }
        public double[] EvaluationPreferences { get; set; }
        public bool AllowNsfw { get; set; }
        public int ColorThemeId { get; set; }
        public virtual ICollection<AppUser>? FriendRequestsSent { get; set; }
        public virtual ICollection<AppUser>? FriendRequestsReceived { get; set; }
    }
}

// tst