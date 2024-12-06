using Loquit.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Services.Abstractions
{
    public interface IPostService
    {
        Task<List<PostDTO>> GetPostsAsync();
        Task<PostDTO> GetPostByIdAsync(int id);
        Task<List<PostDTO>> GetPostsWithTitleAsync(string title);
        Task AddPostAsync(PostDTO post);
        Task DeletePostByIdAsync(int id);
        Task UpdatePostAsync(PostDTO post);
        Task<string> LikePost(int id, string userId);
        Task<string> DislikePost(int id, string userId);
    }
}
