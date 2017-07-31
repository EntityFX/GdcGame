namespace EntityFX.Gdcame.Kernel.Contract
{
    using EntityFX.Gdcame.Kernel.Contract.Counters;

    public abstract class ManualStepResult
    {
        protected ManualStepResult()
        {
            this.IsVerificationRequired = false;
        }

        public bool IsVerificationRequired { get; protected set; }
    }

    public class ManualStepNoVerficationRequiredResult : ManualStepResult
    {
        public ManualStepNoVerficationRequiredResult()
        {
            this.IsVerificationRequired = false;
        }

        public GameCash ModifiedGameCash { get; set; }
    }

    public class ManualStepVerifiedResult : ManualStepResult
    {
        public ManualStepVerifiedResult(bool isValid = false)
        {
            this.IsVerificationRequired = false;
            this.IsVerificationValid = isValid;
        }

        public bool IsVerificationValid { get; protected set; }
    }

    public class ManualStepVerificationRequiredResult : ManualStepResult
    {
        public ManualStepVerificationRequiredResult()
        {
            this.IsVerificationRequired = true;
        }

        public int FirstNumber { get; set; }

        public int SecondNumber { get; set; }
    }


    public class VerificationManualStepData
    {
        public int ResultNumber { get; set; }
    }
}