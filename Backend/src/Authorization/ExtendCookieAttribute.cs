using Backend.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Backend.Authorization;

public class ExtendCookieAttribute : ActionFilterAttribute {
    private readonly TimeSpan _duration;

    public ExtendCookieAttribute(int days, int hours, int minutes, int seconds) {
        _duration = new TimeSpan(days, hours, minutes, seconds);
    }

    public override void OnActionExecuted(ActionExecutedContext context) {
        var response = context.HttpContext.Response;

        if (context.HttpContext.Request.Cookies.TryGetValue("accessToken", out var accessToken)) {
            if (AuthUtils.ParseToken(accessToken, out AuthUtils.TokenParseResult result, out Header? header, out Payload? payload)) {
                response.Cookies.Append("accessToken",
                    TokenGenerator.GenerateAccessToken((long)_duration.TotalSeconds, payload!.Sub, (Role)payload.Role),
                    new CookieOptions {
                        HttpOnly = true,
                        Secure = false,
                        SameSite = SameSiteMode.Lax,
                        Path = "/",
                        Expires = DateTimeOffset.UtcNow.Add(_duration)
                    }
                );
            }
        }

        // if (context.HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken)) {
        //     response.Cookies.Append("refreshToken", refreshToken,
        //         new CookieOptions {
        //             HttpOnly = true,
        //             Secure = false,
        //             SameSite = SameSiteMode.Lax,
        //             Path = "/",
        //             Expires = DateTimeOffset.UtcNow.Add(_duration)
        //         }
        //     );
        // }
    }
}
