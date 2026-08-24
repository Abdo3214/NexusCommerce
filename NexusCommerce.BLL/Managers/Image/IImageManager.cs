using NexusCommerce.BLL.DTOs.Image;
using NexusCommerce.Common.GeneralResult;

namespace NexusCommerce.BLL.Managers.Image
{
    public interface IImageManager
    {
        Task<GeneralResult<ImageUploadResultDto>> UploadImageAsync(ImageUploadDto uploadDto);
    }
}
