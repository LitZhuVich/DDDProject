using DDD.Domain;
using DDD.Domain.Repository;
using DDD.Domain.Services;
using DDD.Infrastructure;
using DDD.Infrastructure.Repository;
using DDD.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.Configure<MvcOptions>(o =>
{
    o.Filters.Add<UnitOfWorkFilter>();
    o.Filters.Add<ApiResponseFilter>();
});
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddScoped<UserDomainService>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddScoped<IUserDomainRepository,UserDomainRepository>();
builder.Services.AddScoped<ISmsCodeSender,MockSmsCodeSender>();

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