using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace DBetter.Frontend;

public class JwtAuthenticationStateProvider(
    HttpClient http,
    ILocalStorageService localStorage) : AuthenticationStateProvider
{
    public static string JwtTokenKey => "jwt_token";
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await localStorage.GetItemAsync<string>(JwtTokenKey);

        var identity = new ClaimsIdentity();

        http.DefaultRequestHeaders.Authorization = null;
        
        if (!string.IsNullOrWhiteSpace(token))
        {
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
        }

        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);
        
        NotifyAuthenticationStateChanged(Task.FromResult(state));
        
        return state;
    }
}