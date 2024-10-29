using CleanDomainValidation.Application;
using DBetter.Application.Users.Commands.Login;
using DBetter.Application.Users.Commands.RefreshJwtToken;
using DBetter.Application.Users.Commands.Register;
using DBetter.Contracts.Users.Commands;
using DBetter.Contracts.Users.Commands.Login;
using DBetter.Contracts.Users.Commands.RefreshJwtTokenParameters;
using MediatR;
namespace DBetter.Api;

public static class AuthenticationModule
{
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

                var jwtToken = result.Value.Item1;
                var refreshToken = result.Value.Item2;
                
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = refreshToken.Expires
                };
                
                context.Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
                
                return Results.Ok(jwtToken);
            })
            .WithName("Login")
            .WithOpenApi();

        app.MapPost("/users/{id}/refresh", async (
            HttpContext context,
            IMediator mediator,
            string id) =>
        {
            var refreshToken = context.Request.Cookies["refreshToken"];
            if (string.IsNullOrWhiteSpace(refreshToken)) return Results.Unauthorized();

            var command = Builder<RefreshJwtTokenCommand>
                .BindParameters(new RefreshJwtTokenParameters())
                .MapParameter(p => p.Id, id)
                .MapParameter(p => p.RefreshToken, refreshToken)
                .BuildUsing<RefreshJwtTokenRequestBuilder>();

            if (command.HasFailed) return Results.BadRequest();

            var result = await mediator.Send(command.Value);
            if (result.HasFailed) return Results.BadRequest();

            var jwtToken = result.Value.Item1;
            var newRefreshToken = result.Value.Item2;

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = newRefreshToken.Expires
            };

            context.Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            return Results.Ok(jwtToken);
        })
            .WithName("Refresh")
            .WithOpenApi();
    }
}