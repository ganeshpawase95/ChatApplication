using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApplication1.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string senderId { get; set; }
        public string receiverId { get; set; }

        [ForeignKey(nameof(senderId))]
        public AppUser Sender { get; set; }
        [ForeignKey(nameof(receiverId))]
        public AppUser Receiver { get; set; }

        public byte[]? FileData { get; set; } // File data
        public string? FileType { get; set; } // Stored file URl/Path
        public string? FileName { get; set; } // identitfy file type
    }
}
