using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Backend.Authorization;

public class JwtAuthFilter : IAuthorizationFilter {
    public void OnAuthorization(AuthorizationFilterContext context) {
        context.HttpContext.Request.Cookies.TryGetValue("accessToken", out var token);

        if (token is null) {
            token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        }

        if (token is null) {
            context.Result = new UnauthorizedResult();
            return;
        }

        var valid = AuthUtils.ParseToken(token, out var result, out var header, out var payload);
        if (!valid) {
            switch (result) {
                case AuthUtils.TokenParseResult.InvalidFormat:
                    context.Result = new UnauthorizedObjectResult(
                        new {
                            message = "Invalid Authorization header"
                        }
                    );
                    break;
                case AuthUtils.TokenParseResult.Invalid:
                    context.Result = new UnauthorizedObjectResult(
                        new {
                            message = "Invalid Authorization header"
                        }
                    );
                    break;
                case AuthUtils.TokenParseResult.TokenExpired:
                    context.Result = new ObjectResult(new { message = "Token expired" }) {
                        StatusCode = 498
                    };
                    break;
                case AuthUtils.TokenParseResult.HeaderNullOrEmpty:
                case AuthUtils.TokenParseResult.PayloadNullOrEmpty:
                case AuthUtils.TokenParseResult.SignatureNullOrEmpty:
                    context.Result = new BadRequestObjectResult(
                        new {
                            message = "Invalid Authorization header"
                        }
                    );
                    break;
                case AuthUtils.TokenParseResult.HeaderDeserializeError:
                    context.Result = new ObjectResult(new { message = "Header deserialization error" }) {
                        StatusCode = 500
                    };
                    break;
                case AuthUtils.TokenParseResult.PayloadDeserializeError:
                    context.Result = new ObjectResult(new { message = "Payload deserialization error" }) {
                        StatusCode = 500
                    };
                    break;
                default:
                    context.Result = new UnauthorizedObjectResult(result.ToString());
                    break;
            }
        }
        else {
            context.HttpContext.Items["jtwHeader"] = header;
            context.HttpContext.Items["jwtPayload"] = payload;
        }
    }
}
