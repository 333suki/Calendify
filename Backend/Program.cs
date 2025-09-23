using Backend;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DatabaseContext>(optionsBuilder => {
    optionsBuilder.UseSqlite("Data Source = database.db");
});

var app = builder.Build();

app.MapGet("/", (DatabaseContext db) => {
    return Results.Ok();
});

app.MapPost("auth/register", (Backend.Dtos.RegisterRequest? registerRequest, DatabaseContext db) => {
    // Empty request body
    if (registerRequest is null) {
        return Results.BadRequest(
            new {
                message = "Empty request body"
            }
        );
    }

    // Username or password not provided in request body
    if (registerRequest.Username is null || registerRequest.Password is null) {
        return Results.BadRequest(
            new {
                message = "Missing credentials"
            }
        );
    }

    // Username already taken
    Backend.Models.User? user = db.Users.FirstOrDefault(user => user.Username == registerRequest.Username);
    if (user is not null) {
        return Results.Conflict(
            new {
                message = "Username already taken"
            }
        );
    }

    db.Users.Add(new Backend.Models.User(registerRequest.Username, Backend.HashUtils.Sha256Hash(registerRequest.Password)));
    db.SaveChanges();
    return Results.Created();
});

app.MapPost("auth/login", (Backend.Dtos.LoginRequest? loginRequest, DatabaseContext db) => {
    // Empty request body
    if (loginRequest is null) {
        return Results.BadRequest(
            new {
                message = "Empty request body"
            }
        );
    }
    
    // Username or password not provided in request body
    if (loginRequest.Username is null || loginRequest.Password is null) {
        return Results.BadRequest(
            new {
                message = "Missing credentials"
            }
        );
    }

    // Username not found
    Backend.Models.User? user = db.Users.FirstOrDefault(user => user.Username == loginRequest.Username);
    if (user is null) {
        return Results.NotFound(
            new {
                message = "No user with that username found"
            }
        );
    }
    
    // Wrong password
    if (HashUtils.Sha256Hash(loginRequest.Password) != user.Password) {
        return Results.Unauthorized();
    }

    return Results.Ok(
        new {
            accessToken = TokenGenerator.GenerateAccessToken(120, user.ID.ToString()),
            refreshToken = TokenGenerator.GenerateRefreshToken()
        }
    );
});

app.Run();
