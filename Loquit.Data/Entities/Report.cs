using Loquit.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Data.Entities
{
    public class Report : BaseEntity
    {
        //a report ticket of a user, post, comment or message
        public string? ReportingUserId { get; set; }
        public virtual AppUser? ReportingUser { get; set; }
        public string? ReportedUserId { get; set; }
        public virtual AppUser? ReportedUser { get; set; }
        public string? AttachedText { get; set; }
        public virtual BaseMessage? ReportedMessage { get; set; }
        public int ReportedMessageId { get; set; }
        public virtual Post? ReportedPost { get; set; }
        public int ReportedPostId { get; set; }
        public virtual Comment? ReportedComment { get; set; }
        public int ReportedCommentId { get; set; }
    }
}
