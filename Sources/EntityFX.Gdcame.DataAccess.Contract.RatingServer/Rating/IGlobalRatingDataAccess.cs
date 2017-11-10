namespace EntityFX.Gdcame.DataAccess.Contract.RatingServer.Rating
{
    //

    using EntityFX.Gdcame.Contract.Common.UserRating;
    using EntityFX.Gdcame.DataAccess.Contract.Common.Rating;

    //
    public interface IGlobalRatingDataAccess : IRatingDataAccess
    {
        //
        void CreateOrUpdateUsersRatingStatistics(TopRatingStatistics ratingStatistics);
        //
        void DropStatistics();
    }
}
