using DevScheduler.API.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DevScheduleCs");
 //builder.Services.AddDbContext<DevScheduleDbContent>(o => o.UseInMemoryDatabase("DevScheduleDB"));
builder.Services.AddDbContext<DevScheduleDbContent>(o => o.UseSqlServer(connectionString));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DevScheduler.API",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "Felipe Martins",
            Email = "fe_mmo@hotmail.com",
            Url = new Uri("https://www.linkedin.com/in/felipe-martins-529a249a/")
        }
    });
    var xmlFile = "DevScheduler.API.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

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
