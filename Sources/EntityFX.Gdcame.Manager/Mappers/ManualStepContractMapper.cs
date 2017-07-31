using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.MainServer.GameManager;
using ManualStepResult = EntityFX.Gdcame.Kernel.Contract.ManualStepResult;

namespace EntityFX.Gdcame.Manager.MainServer.Mappers
{
    using EntityFX.Gdcame.Contract.MainServer.Counters;
    using EntityFX.Gdcame.Kernel.Contract.Counters;

    using ManualStepNoVerficationRequiredResult = EntityFX.Gdcame.Kernel.Contract.ManualStepNoVerficationRequiredResult;
    using ManualStepVerificationRequiredResult = EntityFX.Gdcame.Kernel.Contract.ManualStepVerificationRequiredResult;
    using ManualStepVerifiedResult = EntityFX.Gdcame.Kernel.Contract.ManualStepVerifiedResult;

    public class ManualStepContractMapper : IMapper<ManualStepResult, Contract.MainServer.GameManager.ManualStepResult>
    {
        private static readonly IDictionary<Type, Func<ManualStepResult, Contract.MainServer.GameManager.ManualStepResult>>
            MappingDictionary =
                new ReadOnlyDictionary<Type, Func<ManualStepResult, Contract.MainServer.GameManager.ManualStepResult>>(new Dictionary
                    <Type, Func<ManualStepResult, Contract.MainServer.GameManager.ManualStepResult>>(
                    new Dictionary<Type, Func<ManualStepResult, Contract.MainServer.GameManager.ManualStepResult>>
                    {
                        {
                            typeof (ManualStepNoVerficationRequiredResult), entity => new NoVerficationRequiredResult()
                        },
                        {
                            typeof (ManualStepVerifiedResult), entity => new VerifiedResult
                            {
                                IsVerificationValid = ((ManualStepVerifiedResult) entity).IsVerificationValid
                            }
                        },
                        {
                            typeof (ManualStepVerificationRequiredResult), entity => new VerificationRequiredResult
                            {
                                FirstNumber = ((ManualStepVerificationRequiredResult) entity).FirstNumber,
                                SecondNumber = ((ManualStepVerificationRequiredResult) entity).SecondNumber
                            }
                        }
                    }));

        private readonly IMapper<GameCash, Cash> _countersContractMapper;

        public ManualStepContractMapper(IMapper<GameCash, Cash> countersContractMapper)
        {
            _countersContractMapper = countersContractMapper;
        }

        public Contract.MainServer.GameManager.ManualStepResult Map(ManualStepResult source,
            Contract.MainServer.GameManager.ManualStepResult destination = null)
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