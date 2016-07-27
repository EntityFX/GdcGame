using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.EconomicsArcade.Contract.Common.UserRating;
using EntityFX.EconomicsArcade.Contract.Manager.RatingManager;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.User;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserRating;
using EntityFX.EconomicsArcade.Infrastructure.Repository.UnitOfWork;

namespace EntityFX.EconomicsArcade.DataAccess.Repository
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
