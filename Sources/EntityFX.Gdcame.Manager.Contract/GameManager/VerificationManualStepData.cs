//

namespace EntityFX.Gdcame.Manager.Contract.MainServer.GameManager
{
    using EntityFX.Gdcame.Contract.MainServer.Counters;

    //
    //[KnownType(typeof (NoVerficationRequiredResult))]
    //[KnownType(typeof (VerifiedResult))]
    //[KnownType(typeof (VerificationRequiredResult))]
    public abstract class ManualStepResult
    {
    }

    //
    public class NoVerficationRequiredResult : ManualStepResult
    {
        //
        public Cash ModifiedCash { get; set; }
    }

    //
    public class VerifiedResult : ManualStepResult
    {
        //
        public bool IsVerificationValid { get; set; }
    }

    //
    public class VerificationRequiredResult : ManualStepResult
    {
        //
        public int FirstNumber { get; set; }

        //
        public int SecondNumber { get; set; }
    }
}