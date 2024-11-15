using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using Blazored.LocalStorage;
using DBetter.Contracts.Users.Commands;
using DBetter.Contracts.Users.Commands.Login;
using Microsoft.AspNetCore.Components.Authorization;

namespace DBetter.Frontend;

public class AuthenticationService : AuthenticationStateProvider
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;
    
    public AuthenticationService(IHttpClientFactory httpFactory, ILocalStorageService localStorage)
    {
        _http =httpFactory.CreateClient("DBetter.Api");
        _localStorage = localStorage;
    }
    public static string JwtTokenKey => "jwt_token";
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorage.GetItemAsync<string>(JwtTokenKey);

        var identity = new ClaimsIdentity();
        
        if (!string.IsNullOrWhiteSpace(token))
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
        }

        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);
        
        NotifyAuthenticationStateChanged(Task.FromResult(state));
        
        return state;
    }
    public async Task<RegisterResult> Register(RegisterParameters parameters)
    {
        var response = await _http.PostAsJsonAsync("register", parameters);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("TODO: proper error handling");
        }
        
        var user = await response.Content.ReadFromJsonAsync<RegisterResult>();
        
        if (user is null)
        {
            throw new Exception("TODO: proper error handling");
        }

        return user;
    }
    
    public async Task Login(LoginParameters parameters)
    {
        var response = await _http.PostAsJsonAsync("login", parameters);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("TODO: proper error handling");
        }

        var authData = await response.Content.ReadFromJsonAsync<LoginResult>();
        
        if (authData is null)
        {
            throw new Exception("TODO: proper error handling");
        }
        
        await _localStorage.SetItemAsync(JwtTokenKey, authData.AccessToken);
        await GetAuthenticationStateAsync();
    }
    
    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync(JwtTokenKey);
        await GetAuthenticationStateAsync();
    }
}