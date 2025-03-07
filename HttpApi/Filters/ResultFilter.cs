using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HttpApi.Filters
{
    public class ResultFilter : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var result = context.Result as ObjectResult;
            //返回结果不为空也不是流
            if (result != null && !(result.Value is System.IO.Stream))
            {
                CustomResult customResult = new CustomResult { result = result.Value };
                result.Value = customResult;
            }
            base.OnResultExecuting(context);
        }
    }

    public class CustomResult
    {
        public int errcode { get; set; }
        public string? errmsg { get; set; }

        public object? result { get; set; }
    }
}
