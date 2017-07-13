namespace EntityFX.Gdcame.Application.Contract.Model.MainServer
{
    public class UserSessionsModel
    {
        public string Login { get; set; }

        public SessionInfoModel[] Sessions { get; set; }
    }
}