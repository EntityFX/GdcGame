using System;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.Application.Api.Common.Providers
{
    public class JwtIssuerOptions
    {

        public string SecretKey { get; set; } = "d06da1dbd6184e030e24ff0ee5bd9e66127f216b0db3a36e712df3844eac8d4c";

        public string Issuer { get; set; } = "Gdcame";


        public string Subject { get; set; } = "GdcameMAinServer";


        public string Audience { get; set; } = "GdcameUsers";


        public DateTime NotBefore => DateTime.UtcNow;

        public DateTime IssuedAt => DateTime.UtcNow;


        public TimeSpan ValidFor { get; set; } = TimeSpan.FromHours(12);


        public DateTime Expiration => IssuedAt.Add(ValidFor);


        public Func<Task<string>> JtiGenerator =>
            () => Task.FromResult(Guid.NewGuid().ToString());

    }
}