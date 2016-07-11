﻿namespace EntityFX.EconomicsArcade.Infrastructure.Repository.Criterion
{
    public class GetByIdCriterion : ICriterion
    {
        public int Id { get; set; }

        public GetByIdCriterion(int id)
        {
            Id = id;
        }
    }
}
