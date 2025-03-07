using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HttpApi.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {

        public ExceptionFilter()
        {

        }
        public override async void OnException(ExceptionContext context)
        {
            if (context.Exception != null)
            {
                CustomResult customResult = new CustomResult();
                customResult.errmsg = context.Exception.Message;
                context.Result = new ObjectResult(customResult);
            }
            base.OnException(context);
        }
    }
}
