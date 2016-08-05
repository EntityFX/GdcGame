using EntityFX.Gdcame.Common.Presentation.Model;

namespace EntityFX.Gdcame.Presentation.Web.Model
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