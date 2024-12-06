using AutoMapper;
using Loquit.Data.Entities.ChatTypes;
using Loquit.Data.Repositories.Abstractions;
using Loquit.Services.Abstractions.ChatTypesAbstractions;
using Loquit.Services.DTOs.ChatTypesDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Services.Services.ChatTypesServices
{
    public class DirectChatService : IDirectChatService
    {
        private readonly ICrudRepository<DirectChat> _directChatRepository;
        private readonly IMapper _mapper;
        public DirectChatService(ICrudRepository<DirectChat> directChatRepository, IMapper mapper)
        {
            _directChatRepository = directChatRepository;
            _mapper = mapper;
        }

        public async Task AddDirectChatAsync(DirectChatDTO model)
        {
            var directChat = _mapper
                .Map<DirectChat>(model);

            await _directChatRepository.AddAsync(directChat);
        }

        public async Task DeleteDirectChatByIdAsync(int id)
        {
            await _directChatRepository.DeleteByIdAsync(id);
        }

        public async Task<DirectChatDTO> GetDirectChatByIdAsync(int id)
        {
            var directChat = await _directChatRepository.GetByIdAsync(id);
            return _mapper.Map<DirectChatDTO>(directChat);
        }

        public async Task<List<DirectChatDTO>> GetDirectChatsAsync()
        {
            var directChats = (await _directChatRepository.GetAllAsync())
                .ToList();
            return _mapper.Map<List<DirectChatDTO>>(directChats);
        }

        public async Task UpdateDirectChatAsync(DirectChatDTO model)
        {
            var directChat = _mapper.Map<DirectChat>(model);
            await _directChatRepository.UpdateAsync(directChat);
        }
    }
}
