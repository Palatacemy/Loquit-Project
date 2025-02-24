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
            Chats = new HashSet<ChatUser>();
            SentMessages = new HashSet<BaseMessage>();
            Posts = new HashSet<Post>();
            //Comments = new HashSet<Comment>();
            SavedPosts = new HashSet<Save>();
            LikedPosts = new HashSet<Like>();
            DislikedPosts = new HashSet<Dislike>();
            Friends = new HashSet<AppUser>();
            Blacklist = new HashSet<AppUser>();
            CategoryPreferences = [0, 0.3, 0.3, 0.3, 0.3, 0.3, 0.3, 0.3, 0.3];
            EvaluationPreferences = [0.3, 0.3, 0.3, 0.3, 0.3];
            RecentlyOpenedPostsIds = new int[50];
            AllowNsfw = false;
            ColorThemeId = 1;
            FriendRequestsSent = new HashSet<FriendRequest>();
            FriendRequestsReceived = new HashSet<FriendRequest>();
            ProfilePictureUrl = "user.png";
            PostsRead = 0;
            CommentsWritten = 0;
            PostsWritten = 0;
            MessagesWritten = 0;
        }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Description { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public virtual ICollection<ChatUser>? Chats { get; set; }
        public virtual ICollection<BaseMessage>? SentMessages { get; set; }
        public virtual ICollection<Post>? Posts { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Save>? SavedPosts { get; set; }
        public virtual ICollection<Like>? LikedPosts { get; set; }
        public virtual ICollection<Dislike>? DislikedPosts { get; set; }
        public virtual ICollection<AppUser>? Friends { get; set; }
        public virtual ICollection<AppUser>? Blacklist { get; set; }
        public double[] CategoryPreferences { get; set; }
        public double[] EvaluationPreferences { get; set; }
        public int[] RecentlyOpenedPostsIds { get; set; }
        public bool AllowNsfw { get; set; }
        public int ColorThemeId { get; set; }
        public virtual ICollection<FriendRequest>? FriendRequestsSent { get; set; }
        public virtual ICollection<FriendRequest>? FriendRequestsReceived { get; set; }
        public int PostsRead { get; set; }
        public int CommentsWritten { get; set; }
        public int PostsWritten { get; set; }
        public int MessagesWritten { get; set; }
    }
}

// tst