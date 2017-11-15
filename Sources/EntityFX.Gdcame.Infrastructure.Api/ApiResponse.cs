using System.Net;

namespace EntityFX.Gdcame.Infrastructure.Api
{
    public class ApiResponse<T> : IApiResponse<T>
    {
        public T Data { get; set; }

        public HttpStatusCode HttpCode { get; set; }
    }
}