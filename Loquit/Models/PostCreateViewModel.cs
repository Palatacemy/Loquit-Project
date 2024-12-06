using Loquit.Services.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Loquit.Web.Models
{
    public class PostCreateViewModel : PostDTO
    {
        public IFormFile? Picture { get; set; }
    }
}