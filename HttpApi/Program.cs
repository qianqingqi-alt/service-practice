using HttpApi.Converter;
using HttpApi.Filters;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Infrastructure;
using HttpApi.Configurations;
using Infrastructure.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.EnableEndpointRouting = true;
    //Filter:处理返回结果
    options.Filters.Add<ResultFilter>();
    //Filter:处理异常
    options.Filters.Add<ExceptionFilter>();
    //确保空字符串不被转换为null
    options.ModelBinderProviders.Insert(0, new EmptyStringToNullModelBinderProvider());
})
.AddNewtonsoftJson(options =>
{
    //处理Json格式化：CamelCase小驼峰
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    //处理Json格式化：枚举/字符串转换
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
    //去除所以字符串的前后空格
    options.SerializerSettings.Converters.Add(new StringTrimConverter());
    //处理Json格式化：DateTime/Timestamp转换
    options.SerializerSettings.Converters.Add(new DateTimeToTimestampConverter());
    // 解决ef core 循环引用报错的问题
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(obj =>
{
    //var file = Path.Combine(AppContext.BaseDirectory, "service-practice-api.xml");
    //obj.CustomSchemaIds(s => s.FullName);
    //obj.IncludeXmlComments(file, true);
    //obj.SchemaFilter<SwaggerSchemaFilter>();
});

//注入用户上下文UserContext
builder.Services.AddScoped<UserContext>();

//获取配置文件配置，和命令参数配置 合并为ApplicationConfig
var env = builder.Environment;
builder.Configuration.AddJsonFile($"appsettings.Dev.json", optional: true, reloadOnChange: true);
builder.Services.Configure<ApplicationConfig>(builder.Configuration.GetSection("AppSettings"));

//注入DBContext
builder.Services.AddDatabaseConfiguration(builder.Configuration);
//注入DDD
builder.Services.RegisterServices();

//分布式内存缓存 (AddDistributedMemoryCache) 是框架提供的 IDistributedCache 实现，用于将项存储在内存中。 分布式内存缓存不是真正的分布式缓存。
builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
