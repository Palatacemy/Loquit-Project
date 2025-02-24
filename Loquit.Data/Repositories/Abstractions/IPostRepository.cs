using Loquit.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Data.Repositories.Abstractions
{
    public interface IPostRepository : ICrudRepository<Post>
    {
        Task<List<Post>> GetPostsByAlgorithmAsync(bool AllowNsfw, double[] categoryPreferences, double[] evaluationPreference, int[] recentlyOpenedPostsIds);
    }
}
