using FluentValidation;
using NexusCommerce.BLL.DTOs.Image;
using NexusCommerce.BLL.Mappers.Errors;
using NexusCommerce.Common.GeneralResult;

namespace NexusCommerce.BLL.Managers.Image
{
    public class ImageManager : IImageManager
    {
        private readonly IValidator<ImageUploadDto> _validator;
        private readonly IErrorMapper _errorMapper;

        public ImageManager(IValidator<ImageUploadDto> validator, IErrorMapper errorMapper)
        {
            _validator = validator;
            _errorMapper = errorMapper;
        }

        public async Task<GeneralResult<ImageUploadResultDto>> UploadImageAsync(ImageUploadDto uploadDto)
        {
            var validationResult = await _validator.ValidateAsync(uploadDto);
            if (!validationResult.IsValid)
            {
                return GeneralResult<ImageUploadResultDto>.FailResult(_errorMapper.MapValidationFailure(validationResult));
            }

            try
            {
                var file = uploadDto.File;
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var extension = Path.GetExtension(file.FileName);
                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var url = $"/Files/{fileName}";
                return GeneralResult<ImageUploadResultDto>.SuccessResult(new ImageUploadResultDto { Url = url }, "Image uploaded successfully.");
            }
            catch (Exception ex)
            {
                return GeneralResult<ImageUploadResultDto>.FailResult(Errors.CreateSingle("Image", "UploadFailed", $"Failed to upload image: {ex.Message}"));
            }
        }
    }
}
