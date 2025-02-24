using Loquit.Data.Entities.ChatTypes;
using Loquit.Data.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Data.Repositories
{
    public class GroupChatRepository : CrudRepository<GroupChat>, IGroupChatRepository
    {
        private readonly ApplicationDbContext _context;
        public GroupChatRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
