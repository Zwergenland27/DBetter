using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace DBetter.Frontend;

public class RefreshTokenHandler(
    ILocalStorageService localStorage) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await localStorage.GetItemAsync<string>(AuthenticationService.JwtTokenKey, cancellationToken);

        if (token is not null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        
        return await base.SendAsync(request, cancellationToken);
    }
}