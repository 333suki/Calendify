using Backend.Authorization;
using Backend.Dtos;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ExtendCookie(0, 0, 10, 0)]
[ApiController]
[Route("profile")]
public class ProfileController : ControllerBase {
    private static readonly IProfileService _profileService;

    [ExtendCookie(0, 0, 10, 0)]
    [ServiceFilter(typeof(JwtAuthFilter))]
    [HttpGet("")]
    public IActionResult GetOwnProfileInfo() {

     var payload = HttpContext.Items["jwtPayload"] as Payload;
     User? user = _profileService.GetProfile(Convert.ToInt32(payload!.Sub));

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
    

    // [HttpGet("{userID:int}")]
    // public IActionResult GetProfileInfo(int userID) {
    //     var request = Request;
    //     if (!request.Headers.TryGetValue("Authorization", out var authHeader)) {
    //         return Unauthorized(
    //             new
    //             {
    //                 message = "No Authorization header provided"
    //             }
    //         );
    //     }
        
    //     if (!AuthUtils.ParseToken(authHeader.ToString(), out AuthUtils.TokenParseResult result, out Header? header, out Payload? payload)) {
    //         switch (result) {
    //             case AuthUtils.TokenParseResult.InvalidFormat:
    //                 return BadRequest(
    //                     new
    //                     {
    //                         message = "Invalid Authorization token format"
    //                     }
    //                 );
    //             case AuthUtils.TokenParseResult.Invalid:
    //                 return Unauthorized(
    //                     new
    //                     {
    //                         message = "Invalid Authorization token"
    //                     }
    //                 );
    //             case AuthUtils.TokenParseResult.TokenExpired:
    //                 return StatusCode(498,
    //                     new
    //                     {
    //                         message = "Token expired"
    //                     }
    //                 );
    //             case AuthUtils.TokenParseResult.HeaderNullOrEmpty:
    //             case AuthUtils.TokenParseResult.PayloadNullOrEmpty:
    //             case AuthUtils.TokenParseResult.SignatureNullOrEmpty:
    //                 return BadRequest(
    //                     new
    //                     {
    //                         message = "Invalid Authorization header"
    //                     }
    //                 );
    //             case AuthUtils.TokenParseResult.HeaderDeserializeError:
    //                 return StatusCode(500,
    //                     new
    //                     {
    //                         message = "Header deserialization error"
    //                     }
    //                 );
    //             case AuthUtils.TokenParseResult.PayloadDeserializeError:
    //                 return StatusCode(500,
    //                     new
    //                     {
    //                         message = "Payload deserialization error"
    //                     }
    //                 );
    //         }
    //     }
        
    //     if (payload!.Role!= (int)Role.Admin) {
    //         return Unauthorized(
    //             new {
    //                 message = "User is not admin"
    //             }
    //         );
    //     }


    // met deze kan dus alleen de admin de profile info van een user krijgen (optioneel?)
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

        User? user = _profileService.GetProfile(userID);

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
        
        if (_profileService.UsernameExists(updateRequest.Username) is false) {
            return Conflict(
                new
                {
                    message = "Username already taken"
                }
            );
        }
        
        User? user = _profileService.UpdateProfile(Convert.ToInt32(payload.Sub), updateRequest);

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
                message = "Profile updated successfully"
            }
        );
    }
    

    // [HttpPut("{userID:int}")]
    // public IActionResult UpdateProfile([FromBody] UpdateProfileRequest? updateRequest, int userID) {
    //     var request = Request;
    //     if (!request.Headers.TryGetValue("Authorization", out var authHeader)) {
    //         return Unauthorized(
    //             new
    //             {
    //                 message = "No Authorization header provided"
    //             }
    //         );
    //     }
        
    //     if (!AuthUtils.ParseToken(authHeader.ToString(), out AuthUtils.TokenParseResult result, out Header? header, out Payload? payload)) {
    //         switch (result) {
    //             case AuthUtils.TokenParseResult.InvalidFormat:
    //                 return BadRequest(
    //                     new
    //                     {
    //                         message = "Invalid Authorization token format"
    //                     }
    //                 );
    //             case AuthUtils.TokenParseResult.Invalid:
    //                 return Unauthorized(
    //                     new
    //                     {
    //                         message = "Invalid Authorization token"
    //                     }
    //                 );
    //             case AuthUtils.TokenParseResult.TokenExpired:
    //                 return StatusCode(498,
    //                     new
    //                     {
    //                         message = "Token expired"
    //                     }
    //                 );
    //             case AuthUtils.TokenParseResult.HeaderNullOrEmpty:
    //             case AuthUtils.TokenParseResult.PayloadNullOrEmpty:
    //             case AuthUtils.TokenParseResult.SignatureNullOrEmpty:
    //                 return BadRequest(
    //                     new
    //                     {
    //                         message = "Invalid Authorization header"
    //                     }
    //                 );
    //             case AuthUtils.TokenParseResult.HeaderDeserializeError:
    //                 return StatusCode(500,
    //                     new
    //                     {
    //                         message = "Header deserialization error"
    //                     }
    //                 );
    //             case AuthUtils.TokenParseResult.PayloadDeserializeError:
    //                 return StatusCode(500,
    //                     new
    //                     {
    //                         message = "Payload deserialization error"
    //                     }
    //                 );
    //         }
    //     }

    //     if (payload!.Role!= (int)Role.Admin) {
    //         return Unauthorized(
    //             new {
    //                 message = "User is not admin"
    //             }
    //         );
    //     }

    // [ServiceFilter(typeof(JwtAuthFilter))]
    // [HttpPut("{userID:int}")]
    // public IActionResult UpdateProfile([FromBody] UpdateProfileRequest? updateRequest, int userID) {
    //     var payload = HttpContext.Items["jwtPayload"] as Payload;
    //     if (payload!.Role!= (int)Role.Admin) {
    //         return Unauthorized(
    //             new {
    //                 message = "User is not admin"
    //             }
    //         );
    //     }
    // }

        
    //     if (updateRequest is null) {
    //         return BadRequest(
    //             new
    //             {
    //                 message = "Missing request body"
    //             }
    //         );
    //     }
        
    //     if (updateRequest.Username is null && updateRequest.Password is null && updateRequest.Role is null && updateRequest.Role is null) {
    //         return BadRequest(
    //             new
    //             {
    //                 message = "Empty request body"
    //             }
    //         );
    //     }
        
    //     if (db.Users.FirstOrDefault(u => u.Username == updateRequest.Username) is not null) {
    //         return Conflict(
    //             new
    //             {
    //                 message = "Username already taken"
    //             }
    //         );
    //     }
        
    //     User? user = db.Users.FirstOrDefault(u => u.ID == userID);

    //     if (user is null) {
    //         return NotFound(
    //             new
    //             {
    //                 message = "User not found"
    //             }
    //         );
    //     }

    //     if (updateRequest.Username is not null) {
    //         user.Username = updateRequest.Username;
    //     }
    //     if (updateRequest.Password is not null) {
    //         user.Password = HashUtils.Sha256Hash(updateRequest.Password);
    //     }
    //     if (updateRequest.Role is not null && payload.Role == (int)Role.Admin) {
    //         user.Role = updateRequest.Role.Value;
    //     }

    //     db.SaveChanges();

    //     return Ok(
    //         new
    //         {
    //             message = "Profile updated successfully"
    //         }
    //     );
    // }
    
}