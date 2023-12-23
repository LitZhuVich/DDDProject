using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace DDD.WebApi.Filters
{
    public class ApiResponseFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            // 如果context.Result是ObjectResult，则判断其Value是否为null
            if (context.Result is ObjectResult)
            {
                var objectResult = context.Result as ObjectResult;
                if (objectResult?.Value == null)
                {
                    // 如果为null，则返回BadRequest
                    context.Result = new ObjectResult(ApiResponse.BadRequest());
                }
                else
                {
                    // 如果不为null，则返回Ok
                    context.Result = new ObjectResult(ApiResponse.Ok(objectResult.Value));
                }
            }
            // 如果context.Result是EmptyResult，则返回NotFound
            else if (context.Result is EmptyResult)
            {
                context.Result = new ObjectResult(ApiResponse.NotFound());
            }
            // 如果context.Result是ContentResult，则返回NoContent
            else if (context.Result is ContentResult)
            {
                context.Result = new ObjectResult(ApiResponse.NoContent(((ContentResult)context.Result).Content));
            }
            // 如果context.Result是StatusCodeResult，则返回StatusCodeResult
            else if (context.Result is StatusCodeResult)
            {
                context.Result = new ObjectResult(ApiResponse.StatusCodeResult((StatusCodeResult)context.Result, ""));
            }
        }
    }
}