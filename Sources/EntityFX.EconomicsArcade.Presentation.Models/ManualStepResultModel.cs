using EntityFX.EconomicsArcade.Model.Common.Model;

namespace EntityFX.EconomicsArcade.Presentation.Models
{
    public class ManualStepResultModel
    {
        public VerificationData VerificationData { get; set; }

        public FundsCounterModel ModifiedCountersInfo { get; set; }
    }

    public class VerificationData
    {
        public int FirstNumber { get; set; }
        public int SecondNumber { get; set; }
    }
}