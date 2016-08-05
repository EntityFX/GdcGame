using EntityFX.Gdcame.GameEngine.Contract.Counters;

namespace EntityFX.Gdcame.GameEngine.Contract
{
    public abstract class ManualStepResult
    {
        public bool IsVerificationRequired { get; protected set; }


        protected ManualStepResult()
        {
            IsVerificationRequired = false;
        }
    }

    public class ManualStepNoVerficationRequiredResult : ManualStepResult
    {
        public FundsCounters ModifiedFundsCounters { get; set; }

        public ManualStepNoVerficationRequiredResult()
        {
            IsVerificationRequired = false;
        }
    }

    public class ManualStepVerifiedResult  : ManualStepResult
    {
            
        public bool IsVerificationValid { get; protected set; }

        public ManualStepVerifiedResult(bool isValid = false)
        {
            IsVerificationRequired = false;
            IsVerificationValid = isValid;
        }
    }

    public class ManualStepVerificationRequiredResult : ManualStepResult
    {
        public int FirstNumber { get; set; }

        public int SecondNumber { get; set; }

        public ManualStepVerificationRequiredResult()
        {
            IsVerificationRequired = true;    
        }
    }



    public class VerificationManualStepData
    {
        public int ResultNumber { get; set; }
    }
}