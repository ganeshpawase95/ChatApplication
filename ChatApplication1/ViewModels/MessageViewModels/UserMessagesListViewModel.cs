namespace ChatApplication1.ViewModels.MessageViewModels
{
    public class UserMessagesListViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public bool IsCurrentUserSentMessage { get; set; }
        public string FileName { get; set; }
        public bool IsFile { get; set; }
    }
}
