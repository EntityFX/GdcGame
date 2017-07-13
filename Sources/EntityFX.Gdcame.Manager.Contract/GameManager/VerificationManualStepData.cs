using System.Runtime.Serialization;
using EntityFX.Gdcame.Common.Contract.Counters;

namespace EntityFX.Gdcame.Manager.Contract.MainServer.GameManager
{
    [DataContract]
    [KnownType(typeof (NoVerficationRequiredResult))]
    [KnownType(typeof (VerifiedResult))]
    [KnownType(typeof (VerificationRequiredResult))]
    public abstract class ManualStepResult
    {
    }

    [DataContract]
    public class NoVerficationRequiredResult : ManualStepResult
    {
        [DataMember]
        public Cash ModifiedCash { get; set; }
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