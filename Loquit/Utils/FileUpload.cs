namespace Loquit.Utils
{
    public static class FileUpload
    {
        private static readonly string[] ValidImageMimeTypes = { "image/jpeg", "image/png", "image/gif" };
        private static readonly long MaxFileSize = 15 * 1024 * 1024;

        public static async Task<string> UploadAsync(IFormFile picture, string root)
        {
            if (picture.Length > MaxFileSize)
            {
                throw new InvalidOperationException("File size exceeds the maximum limit of 15MB.");
            }

            if (!IsValidImageType(picture))
            {
                throw new InvalidOperationException("Invalid file type. Only JPEG, PNG and GIF images are allowed.");
            }

            var extension = Path.GetExtension(picture.FileName);
            var name = Guid.NewGuid().ToString();
            var newFileName = $"{name}{extension}";
            var filePath = Path.Combine(root, "uploads", newFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await picture.CopyToAsync(stream);
            }
            return newFileName;
        }

        private static bool IsValidImageType(IFormFile file)
        {
            var mimeType = file.ContentType.ToLower();
            return ValidImageMimeTypes.Contains(mimeType);
        }
    }
}