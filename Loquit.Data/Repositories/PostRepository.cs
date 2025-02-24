using Loquit.Data.Entities;
using Loquit.Data.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Data.Repositories
{
    public class PostRepository : CrudRepository<Post>, IPostRepository
    {
        private readonly ApplicationDbContext _context;
        public PostRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<Post>> GetPostsByAlgorithmAsync(bool allowNsfw, double[] categoryPreferences, double[] evaluationPreferences, int[] recenlyOpedenPostsIds)
        {
            if (!allowNsfw)
            {
                if (true)
                {
                    return _context.Posts
                        .Where(item => item.IsNsfw == false)
                        .Where(item => !recenlyOpedenPostsIds.Contains(item.Id))
                        .OrderBy(item => categoryPreferences[item.CategoryId] + item.Evaluations[0] * evaluationPreferences[0] + item.Evaluations[1] * evaluationPreferences[1] + item.Evaluations[2] * evaluationPreferences[2] + item.Evaluations[3] * evaluationPreferences[3] + item.Evaluations[4] * evaluationPreferences[4]/* + Math.Tanh((item.Likes - item.Dislikes) / 10000)*/)
                        .Take(50)
                        .ToListAsync();
                }
                else
                {
                    return _context.Posts
                        .Where(item => item.IsNsfw == false)
                        .OrderBy(item => categoryPreferences[item.CategoryId] + item.Evaluations[0] * evaluationPreferences[0] + item.Evaluations[1] * evaluationPreferences[1] + item.Evaluations[2] * evaluationPreferences[2] + item.Evaluations[3] * evaluationPreferences[3] + item.Evaluations[4] * evaluationPreferences[4]/* + Math.Tanh((item.Likes - item.Dislikes) / 10000)*/)
                        .Take(50)
                        .ToListAsync();
                }
            }
            else
            {
                if (true)
                {
                    return _context.Posts
                        .Where(item => !recenlyOpedenPostsIds.Contains(item.Id))
                        .OrderBy(item => categoryPreferences[item.CategoryId] + item.Evaluations[0] * evaluationPreferences[0] + item.Evaluations[1] * evaluationPreferences[1] + item.Evaluations[2] * evaluationPreferences[2] + item.Evaluations[3] * evaluationPreferences[3] + item.Evaluations[4] * evaluationPreferences[4]/* + Math.Tanh((item.Likes - item.Dislikes) / 10000)*/)
                        .Take(50)
                        .ToListAsync();
                }
                else
                {
                    return _context.Posts
                        .OrderBy(item => categoryPreferences[item.CategoryId] + item.Evaluations[0] * evaluationPreferences[0] + item.Evaluations[1] * evaluationPreferences[1] + item.Evaluations[2] * evaluationPreferences[2] + item.Evaluations[3] * evaluationPreferences[3] + item.Evaluations[4] * evaluationPreferences[4]/* + Math.Tanh((item.Likes - item.Dislikes) / 10000)*/)
                        .Take(50)
                        .ToListAsync();
                }
            }
        }
    }
}
