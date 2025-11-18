using Backend;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options => {
    options.AddPolicy("AllowLocalhost",
        policy => policy
            .WithOrigins("http://localhost:5173", "http://localhost:5174")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddDbContext<DatabaseContext>(optionsBuilder => {

    optionsBuilder.UseSqlite("Data Source = database.db");
});

builder.Services.AddControllers();

var app = builder.Build();
app.UseCors("AllowLocalhost");
app.MapControllers();

app.Run();
