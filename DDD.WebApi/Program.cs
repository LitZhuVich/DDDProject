using DDD.Domain;
using DDD.Domain.Repository;
using DDD.Domain.Services;
using DDD.Infrastructure;
using DDD.Infrastructure.Repository;
using DDD.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(opt =>
{
    // 忽略循环引用
    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MyDbContext>(o =>
{
    o.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"));
});
// Redis : private readonly ConnectionMultiplexer _Redis;
builder.Services.AddSingleton(provider =>
{
    string? conn = builder.Configuration.GetConnectionString("RedisConnection");

    var configuration = ConfigurationOptions.Parse(conn);
    return ConnectionMultiplexer.Connect(configuration);
});
// 日志
builder.Services.AddLogging(builder =>
{
    //builder.AddConsole();
    builder.AddNLog();
    // 设置最低输出级别的信息
    builder.SetMinimumLevel(LogLevel.Debug);
});
// 拦截器
builder.Services.Configure<MvcOptions>(o =>
{
    o.Filters.Add<UnitOfWorkFilter>();
    o.Filters.Add<ApiResponseFilter>();
});
// 依赖注入
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddScoped<UserDomainService>();
builder.Services.AddScoped<IUserDomainRepository,UserDomainRepository>();
builder.Services.AddScoped<ISmsCodeSender,MockSmsCodeSender>();
// 缓存
builder.Services.AddDistributedMemoryCache();
// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();// 鉴权
app.UseAuthorization();// 授权

#region 配置鉴权
/*builder.Services.AddAuthentication(opt =>
{
    //opt
});*/
#endregion


app.MapControllers();

app.Run();