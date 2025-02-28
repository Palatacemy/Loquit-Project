using AutoMapper;
using Loquit.Data.Entities;
using Loquit.Data.Entities.Abstractions;
using Loquit.Data.Entities.ChatTypes;
using Loquit.Data.Entities.MessageTypes;
using Loquit.Data.Repositories.Abstractions;
using Loquit.Services.DTOs;
using Loquit.Services.DTOs.AbstracionsDTOs;
using Loquit.Services.DTOs.ChatTypesDTOs;
using Loquit.Services.DTOs.MessageTypesDTOs;
using Loquit.Services.Services.Abstractions.ChatTypesAbstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Services.Services.ChatTypesServices
{
    public class DirectChatService : IDirectChatService
    {
        private readonly IDirectChatRepository _directChatRepository;
        private readonly IChatUserRepository _chatUserRepository;
        private readonly ICrudRepository<TextMessage> _textMessageRepository;
        private readonly ICrudRepository<ImageMessage> _imageMessageRepository;
        private readonly IMapper _mapper;
        public DirectChatService(IDirectChatRepository directChatRepository, IChatUserRepository chatUserRepository, ICrudRepository<TextMessage> textMessageRepository, ICrudRepository<ImageMessage> imageMessageRepository, IMapper mapper)
        {
            _directChatRepository = directChatRepository;
            _chatUserRepository = chatUserRepository;
            _textMessageRepository = textMessageRepository;
            _imageMessageRepository = imageMessageRepository;
            _mapper = mapper;
        }

        public async Task AddDirectChatAsync(DirectChatDTO model)
        {
            var directChat = _mapper.Map<DirectChat>(model);

            await _directChatRepository.AddAsync(directChat);

            foreach (var member in model.Members)
            {
                var chatUser = new ChatUser
                {
                    UserId = member.UserId,
                    ChatId = directChat.Id,
                    TimeOfJoining = DateTime.Now
                };

                await _chatUserRepository.AddUserToChatAsync(chatUser);

                _mapper.Map<ChatUserDTO>(chatUser);
            }
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

        public async Task<List<ChatParticipantUserDTO?>> GetUsersInChatAsync(int chatId)
        {
            var chatUsers = await _chatUserRepository.GetUsersInChatAsync(chatId);
            return _mapper.Map<List<ChatParticipantUserDTO?>>(chatUsers.Select(cu => cu.User));
        }

        public async Task<List<DirectChatDTO?>> GetChatsForUserAsync(string userId)
        {
            var chatUsers = await _chatUserRepository.GetChatsForUserAsync(userId);
            return _mapper.Map<List<DirectChatDTO?>>(chatUsers.Select(cu => cu.Chat));
        }

        public async Task<DirectChatDTO> AddMessageToChatAsync(DirectChatDTO directChatDTO, BaseMessageDTO messageDTO)
        {
            var directChat = await _directChatRepository.GetByIdAsync(directChatDTO.Id);

            var messageEntity = MapMessageDTOToEntity(messageDTO, directChat.Id);
            switch (messageEntity)
            {
                case TextMessage textMessage:
                    await _textMessageRepository.AddAsync(textMessage);
                    break;
                case ImageMessage imageMessage:
                    await _imageMessageRepository.AddAsync(imageMessage);
                    break;
                default:
                    throw new ArgumentException("Invalid message type");
            }

            directChat.Messages.Add(messageEntity);
            await _directChatRepository.UpdateAsync(directChat);
            return _mapper.Map<DirectChatDTO>(directChat);
            
        }

        public async Task<DirectChatDTO?> GetDirectChatWithMessagesAsync(int chatId)
        {
            var directChat = await _directChatRepository.GetDirectChatWithMessagesAsync(chatId);
            return _mapper.Map<DirectChatDTO>(directChat);
        }

        public async Task<List<BaseMessageDTO>> GetMessagesForChatAsync(int chatId)
        {
            var messages = await _directChatRepository.GetMessagesByChatIdAsync(chatId);
            return _mapper.Map<List<BaseMessageDTO>>(messages);
        }

        public async Task<List<BaseMessageDTO>> GetMessagesBeforeIdAsync(int chatId, int lastMessageId, int count)
        {
            var messages = await _directChatRepository.GetMessagesBeforeIdAsync(chatId, lastMessageId, count);
            return _mapper.Map<List<BaseMessageDTO>>(messages); // Maps to DTOs
        }

        private BaseMessage MapMessageDTOToEntity(BaseMessageDTO messageDTO, int chatId)
        {
            switch (messageDTO)
            {
                case TextMessageDTO textMessageDTO:
                    return new TextMessage
                    {
                        SenderUserId = textMessageDTO.SenderUserId,
                        Text = textMessageDTO.Text,
                        TimeOfSending = DateTime.Now,
                        ChatId = chatId
                    };

                case ImageMessageDTO imageMessageDTO:
                    return new ImageMessage
                    {
                        SenderUserId = imageMessageDTO.SenderUserId,
                        PictureUrl = imageMessageDTO.PictureUrl,
                        TimeOfSending = DateTime.Now,
                        ChatId = chatId
                    };

                default:
                    throw new ArgumentException("Invalid message type");
            }
        }
    }
}
