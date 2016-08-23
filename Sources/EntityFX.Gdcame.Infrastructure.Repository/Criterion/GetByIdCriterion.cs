namespace EntityFX.Gdcame.Infrastructure.Repository.Criterion
{
    public class GetByIdCriterion : ICriterion
    {
        public GetByIdCriterion(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}