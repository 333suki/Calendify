using Backend;
using Microsoft.EntityFrameworkCore;
using Backend.Services;


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
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventAttendanceService, EventAttendanceService>();
builder.Services.AddScoped<IOfficeAttendanceService, OfficeAttendanceService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IRoomBookingService, RoomBookingService>();



var app = builder.Build();
app.UseCors("AllowLocalhost");
app.MapControllers();

app.Run();
