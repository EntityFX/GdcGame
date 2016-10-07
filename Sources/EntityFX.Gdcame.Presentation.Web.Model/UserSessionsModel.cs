namespace EntityFX.Gdcame.Application.Contract.Model
{
    public class UserSessionsModel
    {
        public string Login { get; set; }

        public SessionInfoModel[] Sessions { get; set; }
    }
}