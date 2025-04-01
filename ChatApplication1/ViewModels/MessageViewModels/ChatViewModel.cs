using ChatApplication1.Models;

namespace ChatApplication1.ViewModels.MessageViewModels
{
    public class ChatViewModel
    {
        public ChatViewModel()
        {
            Messages = new List<UserMessagesListViewModel>();
        }

        public string CurrentUserId { get; set; }
        public string ReceiverId { get; set; }  
        public string ReceicerUserName { get; set; }

        public IEnumerable<UserMessagesListViewModel> Messages { get; set; }

    }

}
