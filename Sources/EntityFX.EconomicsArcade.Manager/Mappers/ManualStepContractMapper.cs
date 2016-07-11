using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.Contract.Game;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using ManualStepResult = EntityFX.EconomicsArcade.Contract.Manager.GameManager.ManualStepResult;

namespace EntityFX.EconomicsArcade.Manager.Mappers
{
    public class ManualStepContractMapper : IMapper<EntityFX.EconomicsArcade.Contract.Game.ManualStepResult, ManualStepResult>
    {
        private readonly IMapper<Contract.Game.Counters.FundsCounters, FundsCounters> _countersContractMapper;

        public ManualStepContractMapper(IMapper<Contract.Game.Counters.FundsCounters, FundsCounters> countersContractMapper)
        {
            _countersContractMapper = countersContractMapper;
        }

        private static readonly IDictionary<Type, Func<Contract.Game.ManualStepResult, ManualStepResult>> MappingDictionary =
    new ReadOnlyDictionary<Type, Func<Contract.Game.ManualStepResult, ManualStepResult>>(new Dictionary<Type, Func<Contract.Game.ManualStepResult, ManualStepResult>>(
        new Dictionary<Type, Func<Contract.Game.ManualStepResult, ManualStepResult>>()
            {
                {
                    typeof(ManualStepNoVerficationRequiredResult), entity => new NoVerficationRequiredResult()
                },
                {
                    typeof(ManualStepVerifiedResult), entity => new VerifiedResult()
                    {
                        IsVerificationValid = ((ManualStepVerifiedResult)entity).IsVerificationValid
                    }
                },
                {
                    typeof(ManualStepVerificationRequiredResult), entity => new VerificationRequiredResult()
                    {
                        FirstNumber = ((ManualStepVerificationRequiredResult)entity).FirstNumber,
                        SecondNumber = ((ManualStepVerificationRequiredResult)entity).SecondNumber,
                    }
                }
            }));
        
        public ManualStepResult Map(Contract.Game.ManualStepResult source, ManualStepResult destination = null)
        {
            var result = MappingDictionary[source.GetType()](source);                     

            var noVerificationResult = result as NoVerficationRequiredResult;
            if (noVerificationResult != null)
            {
                noVerificationResult.ModifiedCounters =
                    _countersContractMapper.Map(((ManualStepNoVerficationRequiredResult) source).ModifiedFundsCounters);
            }
            return result;
        }
    }
}