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
    public class CommentService : ICommentService
    {
        private readonly ICrudRepository<Comment> _commentRepository;
        private readonly ICrudRepository<Like> _likeRepository;
        private readonly ICrudRepository<Dislike> _dislikeRepository;
        private readonly IMapper _mapper;
        public CommentService(ICrudRepository<Comment> commentRepository, ICrudRepository<Like> likeRepository, ICrudRepository<Dislike> dislikeRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _likeRepository = likeRepository;
            _dislikeRepository = dislikeRepository;
            _mapper = mapper;
        }

        public async Task AddCommentAsync(CommentDTO model)
        {
            var comment = _mapper
                .Map<Comment>(model);

            await _commentRepository.AddAsync(comment);
        }

        public async Task DeleteCommentByIdAsync(int id)
        {
            await _commentRepository.DeleteByIdAsync(id);
        }

        public async Task<CommentDTO> GetCommentByIdAsync(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            return _mapper.Map<CommentDTO>(comment);
        }

        public async Task<List<CommentDTO>> GetCommentsAsync()
        {
            var comments = (await _commentRepository.GetAllAsync())
                .ToList();
            return _mapper.Map<List<CommentDTO>>(comments);
        }

        public async Task<List<CommentDTO>> GetCommentsWithTextAsync(string text)
        {
            var comments = (await _commentRepository.GetAsync(item => item.Text == text))
                .ToList();
            return _mapper.Map<List<CommentDTO>>(comments);
        }

        public async Task<string> LikeComment(int commentId, string userId)
        {
            var hasLike = await _likeRepository.GetAsync(item => item.CommentId == commentId && item.UserId == userId);
            if (hasLike.Count() == 0)
            {
                var hasDislike = await _dislikeRepository.GetAsync(item => item.CommentId == commentId && item.UserId == userId);
                var like = new Like()
                {
                    UserId = userId,
                    CommentId = commentId
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

        public async Task<string> DislikeComment(int commentId, string userId)
        {
            var hasDislike = await _dislikeRepository.GetAsync(item => item.CommentId == commentId && item.UserId == userId);
            if (hasDislike.Count() == 0)
            {
                var hasLike = await _likeRepository.GetAsync(item => item.CommentId == commentId && item.UserId == userId);
                var dislike = new Dislike()
                {
                    UserId = userId,
                    CommentId = commentId
                };
                await _dislikeRepository.AddAsync(dislike);
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
        public async Task UpdateCommentAsync(CommentDTO model)
        {
            var comment = _mapper.Map<Comment>(model);
            await _commentRepository.UpdateAsync(comment);
        }
    }
}
