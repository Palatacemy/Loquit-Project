using Loquit.Services.DTOs.AbstracionsDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Services.DTOs.MessageTypesDTOs
{
    public class TextMessageDTO : BaseMessageDTO
    {
        public TextMessageDTO()
        {
            IsEdited = false;
        }
        public string Text { get; set; }
        public bool IsEdited { get; set; }
    }
}
