namespace EntityFX.Gdcame.DataAccess.Repository.Contract.Common.Criterions.User
{
    using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

    public class GetUsersByOffsetCriterion : ICriterion
    {
        public int Offset { get; set; }

        public int Size { get; set; }

        public GetUsersByOffsetCriterion(int offset, int size)
        {
            this.Offset = offset;
            this.Size = size;
        }
    }
}