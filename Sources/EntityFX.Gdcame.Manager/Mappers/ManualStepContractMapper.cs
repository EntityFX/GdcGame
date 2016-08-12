using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.GameEngine.Contract.Counters;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.GameManager;
using ManualStepResult = EntityFX.Gdcame.Manager.Contract.GameManager.ManualStepResult;

namespace EntityFX.Gdcame.Manager.Mappers
{
    public class ManualStepContractMapper : IMapper<GameEngine.Contract.ManualStepResult, ManualStepResult>
    {
        private readonly IMapper<GameCash, Common.Contract.Counters.Cash> _countersContractMapper;

        public ManualStepContractMapper(IMapper<GameCash, Common.Contract.Counters.Cash> countersContractMapper)
        {
            _countersContractMapper = countersContractMapper;
        }

        private static readonly IDictionary<Type, Func<GameEngine.Contract.ManualStepResult, ManualStepResult>> MappingDictionary =
    new ReadOnlyDictionary<Type, Func<GameEngine.Contract.ManualStepResult, ManualStepResult>>(new Dictionary<Type, Func<GameEngine.Contract.ManualStepResult, ManualStepResult>>(
        new Dictionary<Type, Func<GameEngine.Contract.ManualStepResult, ManualStepResult>>()
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
        
        public ManualStepResult Map(GameEngine.Contract.ManualStepResult source, ManualStepResult destination = null)
        {
            var result = MappingDictionary[source.GetType()](source);                     

            var noVerificationResult = result as NoVerficationRequiredResult;
            if (noVerificationResult != null)
            {
                noVerificationResult.ModifiedCash =
                    _countersContractMapper.Map(((ManualStepNoVerficationRequiredResult) source).ModifiedGameCash);
            }
            return result;
        }
    }
}