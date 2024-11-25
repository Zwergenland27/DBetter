using CleanDomainValidation.Application;
using DBetter.Application.Users.Commands.Login;
using DBetter.Application.Users.Commands.RefreshJwtToken;
using DBetter.Application.Users.Commands.Register;
using DBetter.Contracts.Users;
using DBetter.Contracts.Users.Commands;
using DBetter.Contracts.Users.Commands.Login;
using DBetter.Contracts.Users.Commands.RefreshJwtTokenParameters;
using DBetter.Domain.Users.ValueObjects;
using MediatR;

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
                    .BindParameters(parameters)
                    .BuildUsing<RegisterRequestBuilder>();

                if (command.HasFailed) return Results.BadRequest(command.Errors);
                
                var user = await mediator.Send(command.Value);
                if(user.HasFailed) return Results.BadRequest(user.Errors);
                
                return Results.Created($"/users/{user.Value.Id}", user.Value);
            })
            .WithName("Register")
            .WithOpenApi();

        app.MapPost("login", async (
                HttpContext context,
                IMediator mediator,
                LoginParameters parameters) =>
            {
                var command = Builder<LoginCommand>
                    .BindParameters(parameters)
                    .BuildUsing<LoginRequestBuilder>();

                if (command.HasFailed) return Results.BadRequest();
                
                var result = await mediator.Send(command.Value);
                if(result.HasFailed) return Results.BadRequest();
                
                context.AppendRefreshTokenCookie(result.Value.Item2);
                
                return Results.Ok(new AuthenticationDto()
                {
                    Token = result.Value.Item1,
                    RefreshTokenExpiration = result.Value.Item2.Expires
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
                .BindParameters(parameters)
                .MapParameter(p => p.RefreshToken, refreshToken)
                .BuildUsing<RefreshJwtTokenRequestBuilder>();

            if (command.HasFailed) return Results.BadRequest();

            var result = await mediator.Send(command.Value);
            if (result.HasFailed) return Results.BadRequest();
            
            context.AppendRefreshTokenCookie(result.Value.Item2);
            
            return Results.Ok(new AuthenticationDto
            {
                Token = result.Value.Item1,
                RefreshTokenExpiration = result.Value.Item2.Expires
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