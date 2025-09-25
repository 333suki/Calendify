using System.Net;
using Backend;
using Backend.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options => {
    options.AddPolicy("AllowLocalhost", policy => policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddDbContext<DatabaseContext>(optionsBuilder => {
    optionsBuilder.UseSqlite("Data Source = database.db");
});

var app = builder.Build();
app.UseCors("AllowLocalhost");

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
                message = "Invalid username"
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

app.MapPost("auth", (DatabaseContext db, HttpRequest request) => {
    // Check if Authorization header is present
    if (!request.Headers.TryGetValue("Authorization", out var authHeader)) {
        return Results.Unauthorized();
    }

    // Split it on '.' and check if there is 3 parts
    string[] authHeaderParts = authHeader.ToString().Split('.');
    if (authHeaderParts.Length != 3) {
        return Results.BadRequest(
            new {
                message = "Invalid Authorization header"
            }
        );
    }

    // Decode the header (first part) and payload (second part)
    string headerJson = Encoding.UTF8.GetString(TokenGenerator.Base64UrlDecode(authHeaderParts[0]));
    string payloadJson = Encoding.UTF8.GetString(TokenGenerator.Base64UrlDecode(authHeaderParts[1]));
    string signature = authHeaderParts[2];

    if (String.IsNullOrEmpty(headerJson) || String.IsNullOrEmpty(payloadJson) || String.IsNullOrEmpty(signature)) {
        return Results.BadRequest(
            new {
                message = "Invalid Authorization header"
            }
        );
    }
    
    Console.WriteLine(headerJson);
    Console.WriteLine(payloadJson);
    Console.WriteLine(signature);

    // Verify if header and payload generate the correct hash
    if (!TokenGenerator.VerifyToken(authHeader.ToString())) {
        return Results.Unauthorized();
    }

    // Deserialize header JSON into header model
    Backend.Authorization.Header? header = JsonSerializer.Deserialize<Header>(headerJson);
    if (header is null) {
        return Results.InternalServerError(
            new {
                message = "Header deserialization error"
            }
        );
    }
    
    Console.WriteLine($"Alg: {header.Alg}");
    Console.WriteLine($"Typ: {header.Typ}");
    
    // Deserialize payload JSON into payload model
    Backend.Authorization.Payload? payload = JsonSerializer.Deserialize<Payload>(payloadJson);
    if (payload is null) {
        return Results.InternalServerError(
            new {
                message = "Payload deserialization error"
            }
        );
    }
    
    Console.WriteLine($"Sub: {payload.Sub}");
    Console.WriteLine($"Iat: {payload.Iat} ({DateTimeOffset.FromUnixTimeSeconds(payload.Iat)})");
    Console.WriteLine($"Exp: {payload.Exp} ({DateTimeOffset.FromUnixTimeSeconds(payload.Exp)})");
    Console.WriteLine($"Now: {DateTimeOffset.Now.ToUnixTimeSeconds()} ({DateTimeOffset.UtcNow})");
    
    Console.WriteLine($"Issued valid: {DateTimeOffset.FromUnixTimeSeconds(payload.Iat) < DateTimeOffset.UtcNow}");
    Console.WriteLine($"Expired: {DateTimeOffset.FromUnixTimeSeconds(payload.Exp) < DateTimeOffset.UtcNow}");

    // Check if token is expired
    if (DateTimeOffset.FromUnixTimeSeconds(payload.Exp) < DateTimeOffset.UtcNow) {
        return Results.Json(
            statusCode: 498,
            data: new {
                message = "Token expired"
            }
        );
    }
    
    return Results.Ok();
});

app.Run();
