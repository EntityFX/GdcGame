using EntityFX.Gdcame.GameEngine.Contract.Counters;

namespace EntityFX.Gdcame.GameEngine.Contract
{
    public abstract class ManualStepResult
    {
        protected ManualStepResult()
        {
            IsVerificationRequired = false;
        }

        public bool IsVerificationRequired { get; protected set; }
    }

    public class ManualStepNoVerficationRequiredResult : ManualStepResult
    {
        public ManualStepNoVerficationRequiredResult()
        {
            IsVerificationRequired = false;
        }

        public GameCash ModifiedGameCash { get; set; }
    }

    public class ManualStepVerifiedResult : ManualStepResult
    {
        public ManualStepVerifiedResult(bool isValid = false)
        {
            IsVerificationRequired = false;
            IsVerificationValid = isValid;
        }

        public bool IsVerificationValid { get; protected set; }
    }

    public class ManualStepVerificationRequiredResult : ManualStepResult
    {
        public ManualStepVerificationRequiredResult()
        {
            IsVerificationRequired = true;
        }

        public int FirstNumber { get; set; }

        public int SecondNumber { get; set; }
    }


    public class VerificationManualStepData
    {
        public int ResultNumber { get; set; }
    }
}