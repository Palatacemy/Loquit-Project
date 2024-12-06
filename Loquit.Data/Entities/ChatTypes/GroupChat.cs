using Loquit.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Data.Entities.ChatTypes
{
    public class GroupChat : BaseChat
    {
        //a group chat with multiple users
        public string GroupName { get; set; }
        public DateOnly DateOfCreation { get; set; }
        public string PictureUrl { get; set; }
    }
}
