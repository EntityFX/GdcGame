using System.Runtime.Serialization;

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