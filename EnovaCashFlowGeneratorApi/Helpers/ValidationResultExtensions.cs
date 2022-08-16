using FluentValidation.Results;
using System.Linq;

namespace EnovaCashFlowGeneratorApi
{
    public static class ValidationResultExtensions
    {
        public static Result<T> ToResult<T>(this ValidationResult validationResult)
        {
            var result = new Result<T>
            {
                Code = ResultCode.BadRequest,
                Errors = validationResult.Errors.Select(v => new ErrorMessage()
                {
                    PropertyName = v.PropertyName,
                    Message = v.ErrorMessage
                })
            };

            return result;
        }
    }
}