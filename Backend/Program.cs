using System.Net;
using Backend;
using Backend.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

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

var app = builder.Build();
app.UseCors("AllowLocalhost");

app.MapGet("/", ([FromServices] DatabaseContext db) => { return Results.Ok(); });

app.MapPost("auth/register",
    ([FromBody] Backend.Dtos.RegisterRequest? registerRequest, [FromServices] DatabaseContext db) => {
        // Empty request body
        if (registerRequest is null) {
            return Results.BadRequest(
                new
                {
                    message = "Empty request body"
                }
            );
        }

        // Username or password not provided in request body
        if (registerRequest.Username is null || registerRequest.Email is null || registerRequest.Password is null) {
            return Results.BadRequest(
                new
                {
                    message = "Missing credentials"
                }
            );
        }

        // Username already taken
        Backend.Models.User? user = db.Users.FirstOrDefault(user => user.Username == registerRequest.Username);
        if (user is not null) {
            return Results.Conflict(
                new
                {
                    message = "Username already taken"
                }
            );
        }

        db.Users.Add(new Backend.Models.User(registerRequest.Username, registerRequest.Email,
            Backend.HashUtils.Sha256Hash(registerRequest.Password),
            Backend.Models.Role.User));
            
        db.SaveChanges();
        return Results.Created();
    });

app.MapPost("auth/login", ([FromBody] Backend.Dtos.LoginRequest? loginRequest, [FromServices] DatabaseContext db) => {
    // Empty request body
    if (loginRequest is null) {
        return Results.BadRequest(
            new
            {
                message = "Empty request body"
            }
        );
    }

    // Username or password not provided in request body
    if (loginRequest.Username is null || loginRequest.Password is null) {
        return Results.BadRequest(
            new
            {
                message = "Missing credentials"
            }
        );
    }

    // Username not found
    Backend.Models.User? user = db.Users.FirstOrDefault(user => user.Username == loginRequest.Username);
    if (user is null) {
        return Results.NotFound(
            new
            {
                message = "Invalid username"
            }
        );
    }

    // Wrong password
    if (HashUtils.Sha256Hash(loginRequest.Password) != user.Password) {
        return Results.Unauthorized();
    }
    
    // Generate refresh token
    string refreshToken = TokenGenerator.GenerateRefreshToken();
    
    // Check if an entry in the RefreshTokens table is already present for that user
    Backend.Models.RefreshToken? refreshTokenEntry = db.RefreshTokens
        .FirstOrDefault(t => t.UserID == user.ID);
    if (refreshTokenEntry is not null) {
        // If yes, update it
        refreshTokenEntry.Token = refreshToken;
    } else {
        // Else create the entry
        db.RefreshTokens.Add(new Backend.Models.RefreshToken(refreshToken, user.ID));
    }
    db.SaveChanges();

    return Results.Ok(
        new
        {
            accessToken = TokenGenerator.GenerateAccessToken(120, user.ID.ToString(), user.Role),
            refreshToken
        }
    );
});

app.MapPost("auth/authorize", ([FromServices] DatabaseContext db, HttpRequest request) => {
    if (!request.Headers.TryGetValue("Authorization", out var authHeader)) {
        return Results.Unauthorized();
    }

    if (!AuthUtils.ParseToken(authHeader.ToString(), out AuthUtils.TokenParseResult result, out Header? header, out Payload? payload)) {
        switch (result) {
            case AuthUtils.TokenParseResult.InvalidFormat:
                return Results.BadRequest(
                    new
                    {
                        message = "Invalid Authorization header"
                    }
                );
            case AuthUtils.TokenParseResult.Invalid:
                return Results.Unauthorized();
            case AuthUtils.TokenParseResult.TokenExpired:
                return Results.Json(
                    statusCode: 498,
                    data: new
                    {
                        message = "Token expired"
                    }
                );
            case AuthUtils.TokenParseResult.HeaderNullOrEmpty:
            case AuthUtils.TokenParseResult.PayloadNullOrEmpty:
            case AuthUtils.TokenParseResult.SignatureNullOrEmpty:
                return Results.BadRequest(
                    new
                    {
                        message = "Invalid Authorization header"
                    }
                );
            case AuthUtils.TokenParseResult.HeaderDeserializeError:
                return Results.InternalServerError(
                    new
                    {
                        message = "Header deserialization error"
                    }
                );
            case AuthUtils.TokenParseResult.PayloadDeserializeError:
                return Results.InternalServerError(
                    new
                    {
                        message = "Payload deserialization error"
                    }
                );
        }
    }
    
    return Results.Ok();
});


app.MapPost("auth/refresh", ([FromBody] Backend.Dtos.RefreshRequest? refreshRequest, [FromServices] DatabaseContext db, HttpRequest request) => {
    if (!request.Headers.TryGetValue("Authorization", out var authHeader)) {
        return Results.Unauthorized();
    }
    
    if (!AuthUtils.ParseToken(authHeader.ToString(), out AuthUtils.TokenParseResult result, out Header? header, out Payload? payload)) {
        switch (result) {
            case AuthUtils.TokenParseResult.InvalidFormat:
                return Results.BadRequest(
                    new
                    {
                        message = "Invalid Authorization header"
                    }
                );
            case AuthUtils.TokenParseResult.Invalid:
                return Results.Unauthorized();
            case AuthUtils.TokenParseResult.HeaderNullOrEmpty:
            case AuthUtils.TokenParseResult.PayloadNullOrEmpty:
            case AuthUtils.TokenParseResult.SignatureNullOrEmpty:
                return Results.BadRequest(
                    new
                    {
                        message = "Invalid Authorization header"
                    }
                );
            case AuthUtils.TokenParseResult.HeaderDeserializeError:
                return Results.InternalServerError(
                    new
                    {
                        message = "Header deserialization error"
                    }
                );
            case AuthUtils.TokenParseResult.PayloadDeserializeError:
                return Results.InternalServerError(
                    new
                    {
                        message = "Payload deserialization error"
                    }
                );
        }
    }
    
    if (refreshRequest is null) {
        return Results.BadRequest(
            new
            {
                message = "Empty request body"
            }
        );
    }

    string? token = db.RefreshTokens
        .Where(t => t.UserID.ToString() == payload!.Sub)
        .Select(t => t.Token)
        .FirstOrDefault();

    if (token is null) {
        return Results.Unauthorized();
    }

    if (token != refreshRequest.refreshToken) {
        return Results.Unauthorized();
    }
    
    string newRefreshToken = TokenGenerator.GenerateRefreshToken();
    Backend.Models.RefreshToken? refreshTokenEntry = db.RefreshTokens
        .FirstOrDefault(t => t.UserID.ToString() == payload!.Sub);
    if (refreshTokenEntry is null) {
        return Results.NotFound(
            new
            {
                message = $"Refresh token for user with id {payload!.Sub} not found"
            }
        );
    }
    refreshTokenEntry.Token = newRefreshToken;
    db.SaveChanges();

    return Results.Ok(
        new
        {
            accessToken = TokenGenerator.GenerateAccessToken(120, payload!.Sub, (Backend.Models.Role)payload.Role),
            refreshToken = newRefreshToken
        }
    );
});


app.MapPost("/event", ([FromBody] Backend.Dtos.NewEventRequest? newEventRequest, [FromServices] DatabaseContext db, HttpRequest request) => {
    if (!request.Headers.TryGetValue("Authorization", out var authHeader)) {
        return Results.Unauthorized();
    }
    
    if (!AuthUtils.ParseToken(authHeader.ToString(), out AuthUtils.TokenParseResult result, out Header? header, out Payload? payload)) {
        switch (result) {
            case AuthUtils.TokenParseResult.InvalidFormat:
                return Results.BadRequest(
                    new
                    {
                        message = "Invalid Authorization header"
                    }
                );
            case AuthUtils.TokenParseResult.Invalid:
                return Results.Unauthorized();
            case AuthUtils.TokenParseResult.HeaderNullOrEmpty:
            case AuthUtils.TokenParseResult.PayloadNullOrEmpty:
            case AuthUtils.TokenParseResult.SignatureNullOrEmpty:
                return Results.BadRequest(
                    new
                    {
                        message = "Invalid Authorization header"
                    }
                );
            case AuthUtils.TokenParseResult.HeaderDeserializeError:
                return Results.InternalServerError(
                    new
                    {
                        message = "Header deserialization error"
                    }
                );
            case AuthUtils.TokenParseResult.PayloadDeserializeError:
                return Results.InternalServerError(
                    new
                    {
                        message = "Payload deserialization error"
                    }
                );
        }
    }

    if (newEventRequest is null)
    {
        return Results.BadRequest(
            new
            {
                message = "Empty request body"
            }
        );
    }

    // fields not provided in request body
    if (newEventRequest.Title is null || newEventRequest.Description is null || newEventRequest.Date is null)
    {
        return Results.BadRequest(
            new
            {
                message = "Missing event fields"
            }
        );
    }

    db.Events.Add(new Backend.Models.Event(newEventRequest.Title, newEventRequest.Description, newEventRequest.Date));
    db.SaveChanges();

    return Results.Ok();
});

app.Run();
