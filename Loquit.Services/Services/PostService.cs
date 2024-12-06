using AutoMapper;
using Loquit.Data.Entities;
using Loquit.Data.Repositories.Abstractions;
using Loquit.Services.Abstractions;
using Loquit.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Services.Services
{
    public class PostService : IPostService
    {
        private readonly ICrudRepository<Post> _postRepository;
        private readonly ICrudRepository<Like> _likeRepository;
        private readonly ICrudRepository<Dislike> _dislikeRepository;
        private readonly IMapper _mapper;
        public PostService(ICrudRepository<Post> postRepository, ICrudRepository<Like> likeRepository, ICrudRepository<Dislike> dislikeRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _likeRepository = likeRepository;
            _dislikeRepository = dislikeRepository;
            _mapper = mapper;
        }

        public async Task AddPostAsync(PostDTO model)
        {
            var post = _mapper
                .Map<Post>(model);

            await _postRepository.AddAsync(post);
        }

        public async Task DeletePostByIdAsync(int id)
        {
            await _postRepository.DeleteByIdAsync(id);
        }

        public async Task<PostDTO> GetPostByIdAsync(int id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            return _mapper.Map<PostDTO>(post);
        }

        public async Task<List<PostDTO>> GetPostsAsync()
        {
            var posts = (await _postRepository.GetAllAsync())
                .ToList();
            return _mapper.Map<List<PostDTO>>(posts);
        }

        public async Task<List<PostDTO>> GetPostsWithTitleAsync(string title)
        {
            var posts = (await _postRepository.GetAsync(item => item.Title.Contains(title)))
                .ToList();
            return _mapper.Map<List<PostDTO>>(posts);
        }

        public async Task<string> LikePost(int postId, string userId)
        {
            var hasLike = await _likeRepository.GetAsync(item => item.PostId == postId && item.UserId == userId);
            if(hasLike.Count() == 0)
            {

                var hasDislike = await _dislikeRepository.GetAsync(item => item.PostId == postId && item.UserId == userId);
                var like = new Like()
                {
                    UserId = userId,
                    PostId = postId
                };
                await _likeRepository.AddAsync(like);
                if (hasDislike.Count() != 0)
                {
                    await _dislikeRepository.DeleteByIdAsync(hasDislike.First().Id);
                    return "changed";
                }
                else
                {
                    
                    return "added";
                }                   
            }
            else
            {
                await _likeRepository.DeleteByIdAsync(hasLike.First().Id);

                return "removed";
            }
        }

        public async Task<string> DislikePost(int postId, string userId)
        {
            var hasDislike = await _dislikeRepository.GetAsync(item => item.PostId == postId && item.UserId == userId);
            if (hasDislike.Count() == 0)
            {
                var hasLike = await _likeRepository.GetAsync(item => item.PostId == postId && item.UserId == userId);
                var like = new Dislike()
                {
                    UserId = userId,
                    PostId = postId
                };
                await _dislikeRepository.AddAsync(like);
                if (hasLike.Count() != 0)
                {
                    await _likeRepository.DeleteByIdAsync(hasLike.First().Id);
                    return "changed";
                }
                else
                {
                    return "added";
                }
                
            }
            else
            {
                await _dislikeRepository.DeleteByIdAsync(hasDislike.First().Id);

                return "removed";
            }

        }

        public async Task UpdatePostAsync(PostDTO model)
        {
            var post = _mapper.Map<Post>(model);
            await _postRepository.UpdateAsync(post);
        }
    }
}
