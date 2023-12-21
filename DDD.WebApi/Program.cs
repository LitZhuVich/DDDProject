var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication();// ¼øÈ¨
app.UseAuthorization();// ÊÚÈ¨

#region ÅäÖÃ¼øÈ¨
builder.Services.AddAuthentication(opt =>
{
    //opt
});
#endregion


app.MapControllers();

app.Run();