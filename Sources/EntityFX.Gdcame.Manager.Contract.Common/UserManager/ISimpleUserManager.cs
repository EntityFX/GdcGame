﻿namespace EntityFX.Gdcame.Manager.Contract.Common.UserManager
{
    using System.ServiceModel;

    using EntityFX.Gdcame.Contract.Common;

    /// <summary>
    /// </summary>
    [ServiceContract]
    public interface ISimpleUserManager
    {
        [OperationContract]
        bool Exists(string login);

        [OperationContract]
        UserData FindById(string id);

        [OperationContract]
        UserData Find(string login);

        [OperationContract]
        UserData[] FindByFilter(string searchString);

        [OperationContract]
        void Create(UserData login);

        [OperationContract]
        void Delete(string id);
    }
}