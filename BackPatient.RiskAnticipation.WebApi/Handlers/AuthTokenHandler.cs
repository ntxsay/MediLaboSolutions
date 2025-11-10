using System.Net.Http.Headers;

namespace BackPatient.RiskAnticipation.WebApi.Handlers;

public class AuthTokenHandler(IHttpContextAccessor accessor) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = accessor.HttpContext?.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrEmpty(token))
        {
            var cookieToken = accessor.HttpContext?.Request.Cookies["AuthToken"];
            if (!string.IsNullOrEmpty(cookieToken))
            {
                token = $"Bearer {cookieToken}";
            }
        }

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = AuthenticationHeaderValue.Parse(token);
        }
        

        return base.SendAsync(request, cancellationToken);
    }
}