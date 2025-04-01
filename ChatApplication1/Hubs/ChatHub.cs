using ChatApplication1.Data;
using ChatApplication1.Helpers;
using ChatApplication1.Models;
using Microsoft.AspNetCore.SignalR;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ChatApplication1.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public ChatHub(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task SendMessage(string receiverId, string message)
        {
            var NowDate = DateTime.Now;
            var date = NowDate.ToShortDateString();
            var time = NowDate.ToShortTimeString();

            string senderId = _currentUserService.UserId;

            var messageToAdd = new Message()
            {
                Text = message,
                Date = NowDate,
                senderId = senderId,
                receiverId = receiverId,
                FileData = null,   // ✅ Explicitly set null
                FileName = null,   // ✅ Explicitly set null
                FileType = null

            };

            await _context.Messages.AddAsync(messageToAdd);
            await _context.SaveChangesAsync();

            List<string> users = new List<string>()
            {
                receiverId,senderId
            };

            await Clients.Users(users).SendAsync("ReceiveMessage", message, date, time, senderId);
        }

        public async Task ReceiveFile(string fileName, byte[] fileData)
        {
            // Handle the file upload logic here
            // For example, save the file to disk
            // ...
            // Send a confirmation message to the client
            await Clients.Caller.SendAsync("FileReceived", fileName);
        }


        public async Task SendFile(string receiverId, string fileName, string fileType, string base64FileData)
        {
            try
            {
                var senderId = _currentUserService.UserId;
                var NowDate = DateTime.Now;

                // Validate input
                if (string.IsNullOrEmpty(base64FileData))
                {
                    Console.WriteLine("SendFile: Received empty file data.");
                    return;
                }

                byte[] fileData;
                try
                {
                    fileData = Convert.FromBase64String(base64FileData);
                }
                catch (FormatException)
                {
                    Console.WriteLine("SendFile: Invalid Base64 data.");
                    return;
                }

                var fileMessage = new Message()
                {
                    Text = "[file sent]",
                    Date = NowDate,
                    senderId = senderId,
                    receiverId = receiverId,
                    FileData = fileData,
                    FileName = fileName,
                    FileType = fileType
                };

                await _context.Messages.AddAsync(fileMessage);
                await _context.SaveChangesAsync();

                List<string> users = new List<string> { receiverId, senderId };

                // Send the file message to clients
                await Clients.Users(users).SendAsync("ReceiveMessage", "[file sent]", NowDate.ToShortDateString(), NowDate.ToShortTimeString(), senderId, true, fileMessage.FileName);

                // Now send the actual file data
                await Clients.Users(users).SendAsync(
                    "ReceiveFile",
                    fileMessage.Id,
                    fileMessage.FileName,
                    fileMessage.FileType,
                    Convert.ToBase64String(fileMessage.FileData),
                    senderId
                    );
                Console.WriteLine($"Sending File: {fileName}, Size: {fileData.Length} bytes");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SendFile: {ex.Message}");
                throw;
            }
        }


        public async Task<byte[]> GetFile(int messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);
            return message?.FileData;
        }
    }
}
