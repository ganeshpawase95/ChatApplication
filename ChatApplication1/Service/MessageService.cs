using ChatApplication1.Data;
using ChatApplication1.Helpers;
using ChatApplication1.Interface;
using ChatApplication1.Models;
using ChatApplication1.ViewModels.MessageViewModels;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication1.Service
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public MessageService(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task<ChatViewModel> GetMessages(string selectedUserId)
        {
            var currentUserId = _currentUserService.UserId;

            var selectedUser = await _context.Users.FirstOrDefaultAsync(i => i.Id == selectedUserId);
            var selectedUserName = "";
            if (selectedUser != null)
            {
                selectedUserName = selectedUser.UserName;
            }

            var chatViewModel = new ChatViewModel()
            {
                CurrentUserId = currentUserId,
                ReceiverId = selectedUserId,
                ReceicerUserName = selectedUserName,
            };

            
            var messages = await _context.Messages.Where(i => (i.senderId == currentUserId || i.senderId == selectedUserId) &&
                    (i.receiverId == currentUserId || i.receiverId == selectedUserId))
                    .OrderBy(i => i.Date)
                    .Select(i => new UserMessagesListViewModel
            {
                Id = i.Id,
                Text = i.Text,
                Date = i.Date.ToShortDateString(),
                Time = i.Date.ToShortTimeString(),
                IsCurrentUserSentMessage = i.senderId == currentUserId,
                FileName = i.FileName,
                IsFile = i.FileData != null
                    }).ToListAsync();

            chatViewModel.Messages = messages;

            return chatViewModel;
        }

        public async Task<IEnumerable<MessagesUsersListViewModel>> GetUsers()
        {
            var currentUserId = _currentUserService.UserId;

            var users = await _context.Users.Where(i => i.Id != currentUserId).Select(i => new MessagesUsersListViewModel()
            {
                Id = i.Id,
                UserName = i.UserName,
                LastMessage = _context.Messages.Where(m => (m.senderId == currentUserId || m.senderId == i.Id) && (m.receiverId == currentUserId || m.receiverId == i.Id))
                .OrderByDescending(m => m.Date) // Sort by latest message
                .Select(m => m.Text) // Get the message text
                .FirstOrDefault() ?? "No messages yet" // Handle case where no messages exist
            }).ToListAsync();
            return users;
        }

        public async Task SendMessage(string receiverId, string message)
        {
            var senderId = _currentUserService.UserId;
            var newMessage = new Message()
            {
                Text = message,
                Date = DateTime.Now,
                senderId = senderId,
                receiverId = receiverId
            };
            await _context.Messages.AddAsync(newMessage);
            await _context.SaveChangesAsync();
        }

        public async Task SendFile(string receiverId, string fileName, string fileType, byte[] fileData)
        {
            var senderId = _currentUserService.UserId;
            var newMessage = new Message()
            {
                Text = "[file sent]",
                Date = DateTime.Now,
                senderId = senderId,
                receiverId = receiverId,
                FileData = fileData,
                FileName = fileName,
                FileType = fileType
            };
            await _context.Messages.AddAsync(newMessage);
            await _context.SaveChangesAsync();
        }

        public async Task<Message?> GetFile(int messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);
            if (message == null || message.FileData == null)
            {
                return null;
            }
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == messageId && m.FileData != null);
        }
    }
}
