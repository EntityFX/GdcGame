namespace EntityFX.Gdcame.Application.Api.Common
{
    public interface IUserIdentity<TKey>
    {
        string Id { get; set; }
        string UserName { get; set; }
    }
}