using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Common.RestEase
{
    public class CustomHttpClientHandler : HttpMessageHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomHttpClientHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return SendAsync(request, cancellationToken);
        }
    }
}
