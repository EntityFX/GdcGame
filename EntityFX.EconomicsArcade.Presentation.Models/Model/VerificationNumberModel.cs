namespace EntityFX.EconomicsArcade.Presentation.Models
{
    public class VerificationNumberModel
    {
        public VerificationData VerificationData { get; set; }
    }

    public class VerificationData
    {
        public int FirstNumber { get; set; }
        public int SecondNumber { get; set; }
    }
}