using System.Collections.Generic;
using System.Linq;
using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserRating;
using EntityFX.Gdcame.Infrastructure.Repository.UnitOfWork;

namespace EntityFX.Gdcame.DataAccess.Repository
{
    public class UserRatingRepository : IUserRatingRepository
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;



        public UserRatingRepository(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public UserRating[] GetAllUserRatings(GetAllUsersRatingsCriterion findAllUsersRatingsCriterion)
        {

            using (var uow = _unitOfWorkFactory.Create())
            {
                var findQuery = uow.BuildQuery();
                return findQuery.For<IEnumerable<UserRating>>()
                    .With(findAllUsersRatingsCriterion)
                    .ToArray();
            }
        }

       
            //using (var uow = _unitOfWorkFactory.Create())
            //{
            //    var findQuery = uow.BuildQuery();
            //    return findQuery.For<IEnumerable<UserEntity>>()
            //        .With(finalAllCriterion)
            //        .Select(_ => _userContractMapper.Map(_))
            //        .ToArray();
            //}
    }
}
