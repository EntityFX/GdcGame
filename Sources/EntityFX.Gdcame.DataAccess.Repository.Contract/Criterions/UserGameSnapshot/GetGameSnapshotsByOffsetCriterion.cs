using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

namespace EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.UserGameSnapshot
{
    public class GetGameSnapshotsByOffsetCriterion : ICriterion
    {

        public int Offset { get; set; }

        public int Size { get; set; }

        public string[] UserIds { get; private set; }

        public GetGameSnapshotsByOffsetCriterion(int offset, int size, string[] userIds)
        {
            this.Offset = offset;
            this.Size = size;
            this.UserIds = userIds;
        }

    }
}
