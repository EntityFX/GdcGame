namespace EntityFX.Gdcame.Manager.Contract.Common.SessionManager
{
    using System;
    //

    using EntityFX.Gdcame.Contract.Common;

    //
    public interface ISessionManager
    {
        //
        Guid OpenSession(string login);

        //
        bool CloseSession();

        //
        Session GetSession();
    }
}