using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Manager.Contract.MainServer.GameManager
{
    using EntityFX.Gdcame.Contract.MainServer.Counters;

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