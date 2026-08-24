using Microsoft.AspNetCore.Http;

namespace NexusCommerce.BLL.DTOs.Image
{
    public class ImageUploadDto
    {
        public IFormFile File { get; set; } = null!;
    }
}
