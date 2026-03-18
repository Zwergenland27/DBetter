using CleanDomainValidation.Application;
using CleanMediator;
using DBetter.Application;
using DBetter.Application.Users.Commands.Login;
using DBetter.Application.Users.Commands.RefreshJwtToken;
using DBetter.Application.Users.Commands.Register;
using DBetter.Contracts.Users;
using DBetter.Contracts.Users.Commands;
using DBetter.Contracts.Users.Commands.Login;
using DBetter.Contracts.Users.Commands.RefreshJwtTokenParameters;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Api;

public static class AuthenticationModule
{
    private static String RefreshTokenCookieName => "refreshToken";
    public static void AddAuthenticationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("register", async (
            IMediator mediator,
            RegisterParameters parameters) =>
            {
                var command = Builder<RegisterCommand>
                    .WithName("Register")
                    .BindParameters(parameters)
                    .BuildUsing<RegisterRequestBuilder>();
                
                return await mediator.HandleCommandAsync(command, (IUserResult user) =>
                {
                    return Results.Created($"/users/{user.Id}", user);
                });
            })
            .WithName("Register")
            .WithOpenApi();

        app.MapPost("login", async (
                HttpContext context,
                IMediator mediator,
                LoginParameters parameters) =>
            {
                var command = Builder<LoginCommand>
                    .WithName("Login")
                    .BindParameters(parameters)
                    .BuildUsing<LoginRequestBuilder>();

                return await mediator.HandleCommandAsync(command, (Tuple<String, RefreshToken> authData) =>
                {
                    context.AppendRefreshTokenCookie(authData.Item2);
                    
                    return Results.Ok(new AuthenticationDto()
                    {
                        Token = authData.Item1,
                        RefreshTokenExpiration = authData.Item2.Expires.ToIso8601()
                    });
                });
            })
            .WithName("Login")
            .WithOpenApi();

        app.MapPost("refresh", async (
            HttpContext context,
            IMediator mediator,
            RefreshJwtTokenParameters parameters) =>
        {
            var refreshToken = context.Request.Cookies[RefreshTokenCookieName];
            if (string.IsNullOrWhiteSpace(refreshToken)) return Results.Unauthorized();

            var command = Builder<RefreshJwtTokenCommand>
                .WithName("RefreshToken")
                .BindParameters(parameters)
                .MapParameter(p => p.RefreshToken, refreshToken)
                .BuildUsing<RefreshJwtTokenRequestBuilder>();

            return await mediator.HandleCommandAsync(command, (Tuple<String, RefreshToken> authData) =>
            {
                context.AppendRefreshTokenCookie(authData.Item2);
                    
                return Results.Ok(new AuthenticationDto()
                {
                    Token = authData.Item1,
                    RefreshTokenExpiration = authData.Item2.Expires.ToIso8601()
                });
            });
        })
            .WithName("Refresh")
            .WithOpenApi();
    }

    private static void AppendRefreshTokenCookie(this HttpContext context, RefreshToken refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            IsEssential = true,
            Secure = true,
            Expires = refreshToken.Expires,
            SameSite = SameSiteMode.None,
        };
                
        context.Response.Cookies.Append(RefreshTokenCookieName, refreshToken.Token, cookieOptions);
    }
}