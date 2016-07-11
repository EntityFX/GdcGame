using System.Runtime.Serialization;

namespace EntityFX.EconomicsArcade.Contract.Manager.GameManager
{
    [DataContract]
    public class VerificationManualStepResult
    {
        [DataMember]
        public int VerificationNumber { get; set; }
    }
}