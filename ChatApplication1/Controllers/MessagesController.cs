using ChatApplication1.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChatApplication1.Controllers
{
    public class MessagesController : Controller
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _messageService.GetUsers();
            return View(users);
        }

        public async Task<IActionResult> Chat(string selectedUserId)
        {
            var chatViewModel = await _messageService.GetMessages(selectedUserId);
            return View(chatViewModel);
        }

        // To download the file
        public async Task<IActionResult> DownloadFile(int fileId)
        {
            var fileMessage = await _messageService.GetFile(fileId);
            if (fileMessage == null || fileMessage.FileData == null)
            {
                return NotFound("File not found");
            }
            var fileType = fileMessage.FileType ?? "application/octet-stream";
            var fileName = fileMessage.FileName ?? "downloaded-file";

            return File(fileMessage.FileData, fileType, fileName);
        }
    }
}
