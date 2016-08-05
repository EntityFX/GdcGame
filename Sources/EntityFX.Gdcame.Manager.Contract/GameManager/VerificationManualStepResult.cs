using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Manager.Contract.GameManager
{
    [DataContract]
    public class VerificationManualStepResult
    {
        [DataMember]
        public int VerificationNumber { get; set; }
    }
}