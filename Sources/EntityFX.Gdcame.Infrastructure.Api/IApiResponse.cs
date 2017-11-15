using System.Net;

namespace EntityFX.Gdcame.Infrastructure.Api
{
    public interface IApiResponse<T>
    {
        T Data { get; set; }

        HttpStatusCode HttpCode { get; set; }
    }
}