using Demo.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();//Allows Swagger to discover API endpoints.
builder.Services.AddSwaggerGen();//genrate swagger document

var app = builder.Build();

//swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();//swagger/v1/swagger.json
    app.UseSwaggerUI();// swagger / index.html
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();//connect url

app.Run();