using System.Runtime.Serialization;
using EntityFX.EconomicsArcade.Contract.Common.Counters;

namespace EntityFX.EconomicsArcade.Contract.Manager.GameManager
{
    [DataContract]
    [KnownType(typeof(NoVerficationRequiredResult))]
    [KnownType(typeof(VerifiedResult))]
    [KnownType(typeof(VerificationRequiredResult))]
    public abstract class ManualStepResult
    {

    }

    [DataContract]
    public class NoVerficationRequiredResult : ManualStepResult
    {
        [DataMember]
        public FundsCounters ModifiedCounters { get; set; }
    }

    [DataContract]
    public class VerifiedResult : ManualStepResult
    {
        [DataMember]
        public bool IsVerificationValid { get; set; }


    }
    [DataContract]
    public class VerificationRequiredResult : ManualStepResult
    {

        [DataMember]
        public int FirstNumber { get; set; }

        [DataMember]
        public int SecondNumber { get; set; }
    }
}