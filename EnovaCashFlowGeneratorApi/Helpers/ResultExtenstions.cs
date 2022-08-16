using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EnovaCashFlowGeneratorApi
{
    public static class ResultExtenstions
    {
        public static async Task<IActionResult> Process<T>(this Task<Result<T>> resultTask)
        {
            var result = await resultTask;
            switch (result.Code)
            {
                case ResultCode.Ok:
                    return new OkObjectResult(result);
                case ResultCode.BadRequest:
                    return new BadRequestObjectResult(result);
                case ResultCode.NotFound:
                    return new NotFoundObjectResult(result);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static async Task<IActionResult> Process(this Task<Result> resultTask)
        {
            var result = await resultTask;
            switch (result.Code)
            {
                case ResultCode.Ok:
                    return new OkObjectResult(result);
                case ResultCode.BadRequest:
                    return new BadRequestObjectResult(result);
                case ResultCode.NotFound:
                    return new NotFoundObjectResult(result);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}