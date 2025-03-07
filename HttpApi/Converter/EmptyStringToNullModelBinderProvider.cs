using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HttpApi.Converter
{
    public class EmptyStringToNullModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(string))
            {
                return new EmptyStringToNullModelBinder();
            }

            return null;
        }
    }

    public class EmptyStringToNullModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult != ValueProviderResult.None)
            {
                var value = valueProviderResult.FirstValue ?? "";

                // 如果值是空字符串，设置为""
                if (string.IsNullOrEmpty(value))
                {
                    bindingContext.Result = ModelBindingResult.Success("");
                }
                else
                {
                    bindingContext.Result = ModelBindingResult.Success(value);
                }
            }

            return Task.CompletedTask;
        }
    }
}
