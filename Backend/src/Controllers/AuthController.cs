using Backend.Authorization;
using Backend.Dtos;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(DatabaseContext db) : ControllerBase {
    private readonly DatabaseContext db = db;
    // const int TokenDuration = 1200;
    public static readonly TimeSpan TokenDuration = new TimeSpan(0, 0, 10, 0);

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest? loginRequest) {
        // Empty request body
        if (loginRequest is null) {
            return BadRequest(
                new
                {
                    message = "Empty request body"
                }
            );
        }

        // Username or password not provided in request body
        if (loginRequest.Username is null || loginRequest.Password is null) {
            return BadRequest(
                new
                {
                    message = "Missing credentials"
                }
            );
        }

        // Username not found
        User? user = db.Users.FirstOrDefault(user => user.Username == loginRequest.Username);
        if (user is null) {
            return NotFound(
                new
                {
                    message = "Invalid username"
                }
            );
        }

        // Wrong password
        if (HashUtils.Sha256Hash(loginRequest.Password) != user.Password) {
            return Unauthorized();
        }
    
        // Generate refresh token
        string refreshToken = TokenGenerator.GenerateRefreshToken();
        
        // Check if an entry in the RefreshTokens table is already present for that user
        RefreshToken? refreshTokenEntry = db.RefreshTokens
            .FirstOrDefault(t => t.UserID == user.ID);
        if (refreshTokenEntry is not null) {
            // If yes, update it
            refreshTokenEntry.Token = refreshToken;
        } else {
            // Else create the entry
            db.RefreshTokens.Add(new RefreshToken(refreshToken, user.ID));
        }
        db.SaveChanges();

        string accessToken = TokenGenerator.GenerateAccessToken((long)TokenDuration.TotalSeconds, user.ID.ToString(), user.Role);
        // Response.Cookies.Append(
        //     "accessToken",
        //     accessToken,
        //     new CookieOptions {
        //         HttpOnly = true,
        //         Secure = false,
        //         SameSite = SameSiteMode.Lax,
        //         Path = "/",
        //         Expires = DateTimeOffset.UtcNow.Add(TokenDuration)
        //     }
        // );
        // Response.Cookies.Append(
        //     "refreshToken",
        //     refreshToken,
        //     new CookieOptions {
        //         HttpOnly = true,
        //         Secure = false,
        //         SameSite = SameSiteMode.Lax,
        //         Path = "/",
        //         Expires = DateTimeOffset.UtcNow.Add(TokenDuration)
        //     }
        // );
        
        return Ok(
            new
            {
                accessToken,
                refreshToken
            }
        );
    }
    
    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest? registerRequest) {
        // Empty request body
        if (registerRequest is null) {
            return BadRequest(
                new
                {
                    message = "Empty request body"
                }
            );
        }

        // Username or password not provided in request body
        if (registerRequest.Username is null || registerRequest.Email is null || registerRequest.Password is null) {
            return BadRequest(
                new
                {
                    message = "Missing credentials"
                }
            );
        }

        // Username already taken
        User? user = db.Users.FirstOrDefault(user => user.Username == registerRequest.Username);
        if (user is not null) {
            return Conflict(
                new
                {
                    message = "Username already taken"
                }
            );
        }

        db.Users.Add(new User(registerRequest.Username, registerRequest.Email,
            HashUtils.Sha256Hash(registerRequest.Password),
            Role.User));
            
        db.SaveChanges();
        return Created();
    }


    [ServiceFilter(typeof(JwtAuthFilter))]
    [HttpPost("authorize")]
    public IActionResult Authorize() {
        return Ok();
    }

        
    [HttpPost("refresh")]
    public IActionResult Refresh([FromBody] RefreshRequest? refreshRequest) {
        var request = Request;
        if (!request.Headers.TryGetValue("Authorization", out var authHeader)) {
            return Unauthorized();
        }
        
        if (!AuthUtils.ParseToken(authHeader.ToString(), out AuthUtils.TokenParseResult result, out Header? header, out Payload? payload)) {
            switch (result) {
                case AuthUtils.TokenParseResult.InvalidFormat:
                    return BadRequest(
                        new
                        {
                            message = "Invalid Authorization header"
                        }
                    );
                case AuthUtils.TokenParseResult.Invalid:
                    return Unauthorized();
                case AuthUtils.TokenParseResult.HeaderNullOrEmpty:
                case AuthUtils.TokenParseResult.PayloadNullOrEmpty:
                case AuthUtils.TokenParseResult.SignatureNullOrEmpty:
                    return BadRequest(
                        new
                        {
                            message = "Invalid Authorization header"
                        }
                    );
                case AuthUtils.TokenParseResult.HeaderDeserializeError:
                    return StatusCode(500,
                        new
                        {
                            message = "Header deserialization error"
                        }
                    );
                case AuthUtils.TokenParseResult.PayloadDeserializeError:
                    return StatusCode(500,
                        new
                        {
                            message = "Payload deserialization error"
                        }
                    );
            }
        }
        
        if (refreshRequest is null) {
            return BadRequest(
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
            return Unauthorized();
        }
    
        if (token != refreshRequest.refreshToken) {
            return Unauthorized();
        }
        
        string newRefreshToken = TokenGenerator.GenerateRefreshToken();
        RefreshToken? refreshTokenEntry = db.RefreshTokens
            .FirstOrDefault(t => t.UserID.ToString() == payload!.Sub);
        if (refreshTokenEntry is null) {
            return NotFound(
                new
                {
                    message = $"Refresh token for user with id {payload!.Sub} not found"
                }
            );
        }
        refreshTokenEntry.Token = newRefreshToken;
        db.SaveChanges();
    
        string accessToken = TokenGenerator.GenerateAccessToken((long)TokenDuration.TotalSeconds, payload!.Sub, (Role)payload.Role);
        // Response.Cookies.Append(
        //     "accessToken",
        //     accessToken,
        //     new CookieOptions {
        //         HttpOnly = true,
        //         Secure = false,
        //         SameSite = SameSiteMode.Lax,
        //         Path = "/",
        //         Expires = DateTimeOffset.UtcNow.Add(TokenDuration)
        //     }
        // );
        // Response.Cookies.Append(
        //     "refreshToken",
        //     newRefreshToken,
        //     new CookieOptions {
        //         HttpOnly = true,
        //         Secure = false,
        //         SameSite = SameSiteMode.Lax,
        //         Path = "/",
        //         Expires = DateTimeOffset.UtcNow.Add(TokenDuration)
        //     }
        // );
        
        return Ok(
            new
            {
                accessToken,
                newRefreshToken
            }
        );
    }
}
