using ChatApplication1.Models;
using ChatApplication1.ViewModels.MessageViewModels;

namespace ChatApplication1.Interface
{
    public interface IMessageService
    {
        Task<IEnumerable<MessagesUsersListViewModel>> GetUsers();
        Task<ChatViewModel> GetMessages(string selectedUserId);
        Task SendMessage(string receiverId, string message);
        Task SendFile(string receiverId, string fileName, string fileType, byte[] fileData);
        Task<Message?> GetFile(int messageId);

    }
}
