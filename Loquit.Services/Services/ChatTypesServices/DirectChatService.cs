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

        public async Task<DirectChatDTO?> GetDirectChatByIdAsync(int chatId, string userId)
        {
            var directChat = await _directChatRepository.GetByIdAsync(chatId);
            if (directChat == null || !directChat.Members.Any(m => m.UserId == userId))
            {
                return null;
            }
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

        public async Task<DirectChatDTO> AddMessageToChatAsync(int chatId, BaseMessageDTO messageDTO)
        {
            var directChat = await _directChatRepository.GetByIdAsync(chatId);

            var messageEntity = MapMessageDTOToEntity(messageDTO, directChat.Id);
            switch (messageEntity)
            {
                case TextMessage textMessage:
                    await _textMessageRepository.AddAsync(textMessage);
                    messageDTO.Id = textMessage.Id;
                    break;
                case ImageMessage imageMessage:
                    await _imageMessageRepository.AddAsync(imageMessage);
                    messageDTO.Id = imageMessage.Id;
                    break;
                default:
                    throw new ArgumentException("Invalid message type");
            }

            directChat.Messages.Add(messageEntity);
            await _directChatRepository.UpdateAsync(directChat);
            return _mapper.Map<DirectChatDTO>(directChat);
        }

        public async Task<bool> DeleteMessageFromChatAsync(int chatId, int messageId, string userId, bool isImage)
        {
            var directChat = await _directChatRepository.GetDirectChatWithMessagesAndUsersAsync(chatId);

            if (directChat?.Messages == null || !directChat.Messages.Any(m => m.Id == messageId))
            {
                return false;
            }

            var chatUser = directChat.Members.FirstOrDefault(cu => cu.UserId == userId);
            if (chatUser == null)
            {
                return false;
            }

            if (isImage)
            {
                var imageMessage = await _imageMessageRepository.GetByIdAsync(messageId);
                if (imageMessage == null || imageMessage.SenderUserId != userId)
                {
                    return false;
                }
                directChat.Messages.Remove(imageMessage);
                await _imageMessageRepository.DeleteByIdAsync(messageId);
            }
            else
            {
                var textMessage = await _textMessageRepository.GetByIdAsync(messageId);
                if (textMessage == null || textMessage.SenderUserId != userId)
                {
                    return false;
                }
                directChat.Messages.Remove(textMessage);
                await _textMessageRepository.DeleteByIdAsync(messageId);
            }

            await _directChatRepository.UpdateAsync(directChat);

            return true;
        }

        public async Task<BaseMessageDTO?> GetMessageByIdAsync(int messageId)
        {
            var imageMessage = await _imageMessageRepository.GetByIdAsync(messageId);
            if (imageMessage != null)
            {
                return _mapper.Map<ImageMessageDTO>(imageMessage);
            }

            var textMessage = await _textMessageRepository.GetByIdAsync(messageId);
            if (textMessage != null)
            {
                return _mapper.Map<TextMessageDTO>(textMessage);
            }

            return null;
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
