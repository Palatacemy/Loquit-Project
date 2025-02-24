using AutoMapper;
using Loquit.Data;
using Loquit.Data.Entities;
using Loquit.Data.Repositories.Abstractions;
using Loquit.Services.DTOs;
using Loquit.Services.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Loquit.Services.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICrudRepository<FriendRequest> _friendRequestRepository;
        private readonly IMapper _mapper;

        public UserService(UserManager<AppUser> userManager, ICrudRepository<FriendRequest> friendrequestRepository, IMapper mapper)
        {
            _userManager = userManager;
            _friendRequestRepository = friendrequestRepository;
            _mapper = mapper;
        }

        public async Task<IdentityResult> CreateUserAsync(AppUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<List<AppUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<AppUser?> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<List<AppUser>> GetUserWithNameAsync(string username)
        {
            var users = await _userManager.Users.Where(item => item.UserName.Contains(username)).ToListAsync();
            return users;
        }

        public async Task<IdentityResult> UpdateUserAsync(AppUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                return await _userManager.DeleteAsync(user);
            }
            return IdentityResult.Failed(new IdentityError { Description = "User not found" });
        }

        public async Task<string> SendFriendRequest(string senderId, string recieverId)
        {
            var reciever = await _userManager.FindByIdAsync(recieverId);
            var sender = await _userManager.FindByIdAsync(senderId);

            var friendRequest = new FriendRequest()
            {
                SentToUserId = recieverId,
                SentByUserId = senderId
            };
            await _friendRequestRepository.AddAsync(friendRequest);
            return "success";
        }

        public async Task<string> DeclineFriendRequest(string senderId, string recieverId)
        {
            var hasRequest = await _friendRequestRepository.GetAsync(e => e.SentByUserId == senderId && e.SentToUserId == recieverId);
            await _friendRequestRepository.DeleteByIdAsync(hasRequest.First().Id);
            return "success";
        }

        public async Task<string> AcceptFriendRequest(string senderId, string recieverId)
        {
            var reciever = await _userManager.FindByIdAsync(recieverId);
            var sender = await _userManager.FindByIdAsync(senderId);
            reciever.Friends.Add(sender);
            sender.Friends.Add(reciever);
            await _userManager.UpdateAsync(sender);
            await _userManager.UpdateAsync(reciever);
            var hasRequest = await _friendRequestRepository.GetAsync(e => e.SentByUserId == senderId && e.SentToUserId == recieverId);
            await _friendRequestRepository.DeleteByIdAsync(hasRequest.First().Id);
            return "success";
        }

        public async Task<string> CancelFriendRequest(string senderId, string recieverId)
        {
            var hasRequest = await _friendRequestRepository.GetAsync(e => e.SentByUserId == senderId && e.SentToUserId == recieverId);
            await _friendRequestRepository.DeleteByIdAsync(hasRequest.First().Id);
            return "success";
        }

        public async Task<string> RemoveFriend(string senderId, string recieverId)
        {
            var reciever = await _userManager.FindByIdAsync(recieverId);
            var sender = await _userManager.FindByIdAsync(senderId);
            reciever.Friends.Remove(sender);
            sender.Friends.Remove(reciever);
            await _userManager.UpdateAsync(sender);
            await _userManager.UpdateAsync(reciever);
            return "success";
        }

        public async Task<ChatParticipantUserDTO?> GetChatParticipantUserDTOByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user != null ? _mapper.Map<ChatParticipantUserDTO>(user) : null;
        }
    }
}

