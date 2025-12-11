using Backend.Authorization;
using Backend.Dtos;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ExtendCookie(0, 0, 10, 0)]
[ApiController]
[Route("profile")]
public class ProfileController(DatabaseContext db) : ControllerBase {
    private readonly DatabaseContext db = db;

    [ExtendCookie(0, 0, 10, 0)]
    [ServiceFilter(typeof(JwtAuthFilter))]
    [HttpGet("")]
    public IActionResult GetOwnProfileInfo() {
        var payload = HttpContext.Items["jwtPayload"] as Payload;
        User? user = db.Users.FirstOrDefault(u => u.ID == Convert.ToInt32(payload.Sub));

        if (user is null) {
            return NotFound();
        }

        return Ok(
            new
            {
                username = user.Username,
                email = user.Email,
                role = user.Role
            }
        );
    }
    
    [ServiceFilter(typeof(JwtAuthFilter))]
    [HttpGet("{userID:int}")]
    public IActionResult GetProfileInfo(int userID) {
        var payload = HttpContext.Items["jwtPayload"] as Payload;
        if (payload!.Role!= (int)Role.Admin) {
            return Unauthorized(
                new {
                    message = "User is not admin"
                }
            );
        }

        User? user = db.Users.FirstOrDefault(u => u.ID == userID);

        if (user is null) {
            return NotFound(
                new
                {
                    message = "User not found"
                }
            );
        }

        return Ok(
            new
            {
                username = user.Username,
                email = user.Email,
                role = user.Role
            }
        );
    }

    [ServiceFilter(typeof(JwtAuthFilter))]
    [HttpPut("")]
    public IActionResult UpdateOwnProfile([FromBody] UpdateProfileRequest? updateRequest) {
        var payload = HttpContext.Items["jwtPayload"] as Payload;
        if (updateRequest is null) {
            return BadRequest(
                new
                {
                    message = "Missing request body"
                }
            );
        }
        
        if (updateRequest.Username is null && updateRequest.Password is null && updateRequest.Role is null) {
            return BadRequest(
                new
                {
                    message = "Empty request body"
                }
            );
        }
        
        if (payload!.Role != (int)Role.Admin && updateRequest.Role is not null) {
            return Unauthorized(
                new
                {
                    message = "Non-admin cannot update role"
                }
            );
        }
        
        if (db.Users.FirstOrDefault(u => u.Username == updateRequest.Username) is not null) {
            return Conflict(
                new
                {
                    message = "Username already taken"
                }
            );
        }
        
        User? user = db.Users.FirstOrDefault(u => u.ID == Convert.ToInt32(payload.Sub));

        if (user is null) {
            return NotFound(
                new
                {
                    message = "User not found"
                }
            );
        }

        if (updateRequest.Username is not null) {
            user.Username = updateRequest.Username;
        }
        if (updateRequest.Password is not null) {
            user.Password = HashUtils.Sha256Hash(updateRequest.Password);
        }
        if (updateRequest.Role is not null && payload.Role == (int)Role.Admin) {
            user.Role = updateRequest.Role.Value;
        }

        db.SaveChanges();

        return Ok(
            new
            {
                message = "Profile updated successfully"
            }
        );
    }
    
    [ServiceFilter(typeof(JwtAuthFilter))]
    [HttpPut("{userID:int}")]
    public IActionResult UpdateProfile([FromBody] UpdateProfileRequest? updateRequest, int userID) {
        var payload = HttpContext.Items["jwtPayload"] as Payload;
        if (payload!.Role!= (int)Role.Admin) {
            return Unauthorized(
                new {
                    message = "User is not admin"
                }
            );
        }
        
        if (updateRequest is null) {
            return BadRequest(
                new
                {
                    message = "Missing request body"
                }
            );
        }
        
        if (updateRequest.Username is null && updateRequest.Password is null && updateRequest.Role is null && updateRequest.Role is null) {
            return BadRequest(
                new
                {
                    message = "Empty request body"
                }
            );
        }
        
        if (db.Users.FirstOrDefault(u => u.Username == updateRequest.Username) is not null) {
            return Conflict(
                new
                {
                    message = "Username already taken"
                }
            );
        }
        
        User? user = db.Users.FirstOrDefault(u => u.ID == userID);

        if (user is null) {
            return NotFound(
                new
                {
                    message = "User not found"
                }
            );
        }

        if (updateRequest.Username is not null) {
            user.Username = updateRequest.Username;
        }
        if (updateRequest.Password is not null) {
            user.Password = HashUtils.Sha256Hash(updateRequest.Password);
        }
        if (updateRequest.Role is not null && payload.Role == (int)Role.Admin) {
            user.Role = updateRequest.Role.Value;
        }

        db.SaveChanges();

        return Ok(
            new
            {
                message = "Profile updated successfully"
            }
        );
    }
}
