namespace EntityFX.Gdcame.Engine.Contract.RatingServer
{
    using System;

    using EntityFX.Gdcame.Contract.Common.UserRating;

    public interface INodeRatingClientFactory
    {
        IRatingDataRetrieve BuildClient(Uri nodeUri);
    }
}