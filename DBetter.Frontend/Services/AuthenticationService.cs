using System.Net.Http.Json;
using Blazored.LocalStorage;
using DBetter.Contracts.Users.Commands;
using DBetter.Contracts.Users.Commands.Login;
using Microsoft.AspNetCore.Components.Authorization;

namespace DBetter.Frontend;

public class AuthenticationService(
    HttpClient http,
    AuthenticationStateProvider authenticationStateProvider,
    ILocalStorageService localStorage)
{
    public async Task<RegisterResult> Register(RegisterParameters parameters)
    {
        var response = await http.PostAsJsonAsync("register", parameters);
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
        var response = await http.PostAsJsonAsync("login", parameters);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("TODO: proper error handling");
        }

        var authData = await response.Content.ReadFromJsonAsync<LoginResult>();
        
        if (authData is null)
        {
            throw new Exception("TODO: proper error handling");
        }
        
        await localStorage.SetItemAsync(JwtAuthenticationStateProvider.JwtTokenKey, authData.AccessToken);
        await authenticationStateProvider.GetAuthenticationStateAsync();
    }
}