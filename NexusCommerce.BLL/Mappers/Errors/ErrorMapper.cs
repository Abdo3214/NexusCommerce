using FluentValidation.Results;
using NexusCommerce.Common.GeneralResult;

namespace NexusCommerce.BLL.Mappers.Errors
{
    public class ErrorMapper : IErrorMapper
    {
        public Dictionary<string, List<NexusCommerce.Common.GeneralResult.Errors>> MapValidationFailure(ValidationResult validationResult)
        {
            var dictionary = new Dictionary<string, List<NexusCommerce.Common.GeneralResult.Errors>>();

            foreach (var failure in validationResult.Errors)
            {
                var key = failure.PropertyName;
                var error = new NexusCommerce.Common.GeneralResult.Errors(failure.ErrorCode ?? "Validation", failure.ErrorMessage);

                if (!dictionary.ContainsKey(key))
                {
                    dictionary[key] = new List<NexusCommerce.Common.GeneralResult.Errors>();
                }

                dictionary[key].Add(error);
            }

            return dictionary;
        }
    }
}
