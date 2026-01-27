using System.Net.Http.Headers;

namespace FrontPatient.AspNetCore.Handlers;

/// <summary>
/// Composant permettant de récupérer le token JWT au sein du cookie et le met dans l'en-tête Authorization de la requête. Elle permet ainsi de simplifier les appels d'Api des microservices qui nécessitent un Token d'authentification.
/// </summary>
/// <param name="accessor">Contient les informations de la requête en cours</param>
public class AuthTokenHandler(IHttpContextAccessor accessor) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = accessor.HttpContext?.Request.Cookies["AuthToken"];

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return base.SendAsync(request, cancellationToken);
    }
}