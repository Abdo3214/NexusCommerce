using FluentValidation.Results;
using NexusCommerce.Common.GeneralResult;

namespace NexusCommerce.BLL.Mappers.Errors
{
    public interface IErrorMapper
    {
        Dictionary<string, List<NexusCommerce.Common.GeneralResult.Errors>> MapValidationFailure(ValidationResult validationResult);
    }
}
