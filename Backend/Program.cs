using Backend;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Backend.Models;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DatabaseContext>(optionsBuilder =>
{
    optionsBuilder.UseSqlite("Data Source = database.db");

});

var app = builder.Build();



app.MapGet("/", (DatabaseContext db) =>
{
    //db.Users.Add(new Backend.Models.User("admin", "admin123"));
    //db.SaveChanges();
    return Results.Ok();


});

app.MapPost("auth/register", (Backend.Dtos.RegisterRequest? registerRequest, DatabaseContext db) =>
{
    //db.Users.Add(new Backend.Models.User("admin", "admin123"));
    //db.SaveChanges();

    if (registerRequest is null)
    {
        return Results.BadRequest();
    }

    if (registerRequest.Username is null || registerRequest.Password is null)
    {
        return Results.BadRequest();
    }

    Backend.Models.User? user = db.Users.FirstOrDefault(user => user.Username == registerRequest.Username);

    if (user is not null)
    {
        return Results.Conflict();
    }

    db.Users.Add(new Backend.Models.User(registerRequest.Username, registerRequest.Password));
    db.SaveChanges();
    return Results.Created();
});

app.MapPost("auth/login", (Backend.Dtos.LoginRequest? loginRequest, DatabaseContext db) =>
{
    //db.Users.Add(new Backend.Models.User("admin", "admin123"));
    //db.SaveChanges();

    if (loginRequest is null)
    {
        return Results.BadRequest();
    }


    if (loginRequest.Username is null || loginRequest.Password is null)
    {
        return Results.BadRequest();
    }

    Backend.Models.User? user = db.Users.FirstOrDefault(user => user.Username == loginRequest.Username);

    if (user is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(
    );


});


app.Run();