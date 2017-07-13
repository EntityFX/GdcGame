using EntityFX.Gdcame.Common.Application.Model;

namespace EntityFX.Gdcame.Application.Contract.Model.MainServer
{
    public class ManualStepResultModel
    {
        public VerificationData VerificationData { get; set; }

        public CashModel ModifiedCountersInfo { get; set; }
    }

    public class VerificationData
    {
        public int FirstNumber { get; set; }
        public int SecondNumber { get; set; }
    }
}