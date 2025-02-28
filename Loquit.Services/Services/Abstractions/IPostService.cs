using Loquit.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Services.Services.Abstractions
{
    public interface IPostService
    {
        Task<List<PostDTO>> GetPostsAsync();
        Task<List<PostDTO>> GetPostsByAlgorithmAsync(bool allowNsfw, double[] categoryPreferences, double[] evaluationPreference, int[] recentlyOpenedPostsIds);
        Task<PostDTO> GetPostByIdAsync(int id);
        Task<List<PostDTO>> GetPostsWithTitleAsync(string title);
        Task AddPostAsync(PostDTO post);
        Task DeletePostByIdAsync(int id);
        Task UpdatePostAsync(PostDTO post);
        double[] Evaluate(PostDTO model);
        Task<string> LikePost(int id, string userId);
        Task<string> DislikePost(int id, string userId);
        Task<DateTime> GetTimeOfPostingAsync(int id);
    }
}
