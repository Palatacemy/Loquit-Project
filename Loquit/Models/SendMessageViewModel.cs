using Loquit.Services.DTOs;
using Loquit.Services.DTOs.AbstracionsDTOs;

namespace Loquit.Web.Models
{
    public class SendMessageViewModel
    {
        public int ChatId { get; set; }
        public string MessageType { get; set; }
        public string Text { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
