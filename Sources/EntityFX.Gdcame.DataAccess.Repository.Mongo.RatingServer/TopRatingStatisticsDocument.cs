namespace EntityFX.Gdcame.DataAccess.Repository.Mongo.RatingServer
{
    using System;

    using MongoDB.Bson;

    class TopRatingStatisticsDocument
    {
        public ObjectId Id { get; set; }
        public string UserId { get; set; }
        public string Login { get; set; }
        public decimal Value { get; set; }
        public int CounterType { get; set; }
        public int PeriodType { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}