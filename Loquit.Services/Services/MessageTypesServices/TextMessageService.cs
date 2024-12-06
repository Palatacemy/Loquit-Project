using AutoMapper;
using Loquit.Data.Entities.MessageTypes;
using Loquit.Data.Repositories.Abstractions;
using Loquit.Services.Abstractions.MessageTypesAbstractions;
using Loquit.Services.DTOs.MessageTypesDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Services.Services.MessageTypesServices
{
    public class TextMessageService : ITextMessageService
    {
        private readonly ICrudRepository<TextMessage> _textMessageRepository;
        private readonly IMapper _mapper;
        public TextMessageService(ICrudRepository<TextMessage> textMessageRepository, IMapper mapper)
        {
            _textMessageRepository = textMessageRepository;
            _mapper = mapper;
        }

        public async Task AddTextMessageAsync(TextMessageDTO model)
        {
            var textMessage = _mapper
                .Map<TextMessage>(model);

            await _textMessageRepository.AddAsync(textMessage);
        }

        public async Task DeleteTextMessageByIdAsync(int id)
        {
            await _textMessageRepository.DeleteByIdAsync(id);
        }

        public async Task<TextMessageDTO> GetTextMessageByIdAsync(int id)
        {
            var textMessage = await _textMessageRepository.GetByIdAsync(id);
            return _mapper.Map<TextMessageDTO>(textMessage);
        }

        public async Task<List<TextMessageDTO>> GetTextMessagesAsync()
        {
            var textMessages = (await _textMessageRepository.GetAllAsync())
                .ToList();
            return _mapper.Map<List<TextMessageDTO>>(textMessages);
        }

        public async Task UpdateTextMessageAsync(TextMessageDTO model)
        {
            var textMessage = _mapper.Map<TextMessage>(model);
            await _textMessageRepository.UpdateAsync(textMessage);
        }
    }
}
