using HttpApi.Converter;
using HttpApi.Filters;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.EnableEndpointRouting = true;
    //Filter:�����ؽ��
    options.Filters.Add<ResultFilter>();
    //Filter:�����쳣
    options.Filters.Add<ExceptionFilter>();
    //ȷ�����ַ�������ת��Ϊnull
    options.ModelBinderProviders.Insert(0, new EmptyStringToNullModelBinderProvider());
})
.AddNewtonsoftJson(options =>
{
    //����Json��ʽ����CamelCaseС�շ�
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    //����Json��ʽ����ö��/�ַ���ת��
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
    //ȥ�������ַ�����ǰ��ո�
    options.SerializerSettings.Converters.Add(new StringTrimConverter());
    //����Json��ʽ����DateTime/Timestampת��
    options.SerializerSettings.Converters.Add(new DateTimeToTimestampConverter());
    // ���ef core ѭ�����ñ��������
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//ע���û�������UserContext
builder.Services.AddScoped<UserContext>();

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
