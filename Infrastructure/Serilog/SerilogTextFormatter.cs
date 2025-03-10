using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Formatting;

namespace Infrastructure.Serilog
{
    public class SerilogTextFormatter : ITextFormatter
    {
        private readonly string _jsonObjectGenerator;

        // 构造函数，允许用户自定义 jsonObject 的结构
        public SerilogTextFormatter(string jsonObjectGenerator)
        {
            _jsonObjectGenerator = jsonObjectGenerator;
        }

        public void Format(LogEvent logEvent, TextWriter output)
        {
            try
            {
                var buffer = new StringWriter();
                FormatContent(logEvent, buffer);
                output.WriteLine(buffer.ToString());
            }
            catch (System.Exception e)
            {
                LogNonFormattableEvent(logEvent, e);
            }
        }

        private void FormatContent(LogEvent logEvent, TextWriter output)
        {
            var properties = new Dictionary<string, object>();
            var Properties = logEvent.Properties;
            var source = "";
            var sourceType = "";
            foreach (var item in Properties)
            {
                if (item.Key != "Custom.Source" && item.Key != "Custom.SourceType") properties[item.Key] = item.Value?.ToString() ?? "";
                if (item.Key == "Custom.Source") source = item.Value?.ToString() ?? "";
                if (item.Key == "Custom.SourceType") sourceType = item.Value?.ToString() ?? "";
            }

            var replacements = new Dictionary<string, object>
            {
                { "{Level}", logEvent.Level },
                { "{Exception}", logEvent.Exception!= null ? logEvent.Exception : "" },
                { "{SpanId}", logEvent.SpanId != null ? logEvent.SpanId.Value.ToString() : "" },
                { "{Timestamp}", logEvent.Timestamp },
                { "{MessageTemplate}", logEvent.MessageTemplate },
                { "{TraceId}", logEvent.TraceId != null ? logEvent.TraceId.Value.ToString() : "" },
                { "{Properties}", properties },
                { "{Message}", logEvent.RenderMessage() },
                { "{Custom.Source}", source },
                { "{Custom.SourceType}", sourceType },
            };
            var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(_jsonObjectGenerator);
            ReplaceValue(jsonObject, replacements);
            var json = JsonConvert.SerializeObject(jsonObject);
            output.Write(json);
        }

        private static void LogNonFormattableEvent(LogEvent logEvent, System.Exception e)
        {
            SelfLog.WriteLine(
              "Event at {0} with message template {1} could not be formatted into JSON and will be dropped: {2}",
              logEvent.Timestamp.ToString("o"),
              logEvent.MessageTemplate.Text,
              e);
        }

        private static Dictionary<string, object> ReplaceValue(Dictionary<string, object> jsonObject, Dictionary<string, object> replacements)
        {
            // 遍历字典并检查是否有匹配的值
            foreach (var key in new List<string>(jsonObject.Keys))
            {
                var value = jsonObject[key];
                if (value is JObject subObject)  // 如果值是字典，递归遍历
                {
                    var subObjectReplace = ReplaceValue(subObject.ToObject<Dictionary<string, object>>(), replacements);
                    jsonObject[key] = subObjectReplace;
                }
                else if (value is string stringValue && replacements.ContainsKey(stringValue))
                {
                    jsonObject[key] = replacements[stringValue];  // 替换为对应的值
                }
            }
            return jsonObject;
        }
    }
}
