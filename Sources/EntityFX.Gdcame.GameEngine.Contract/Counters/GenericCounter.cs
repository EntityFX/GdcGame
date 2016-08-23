namespace EntityFX.Gdcame.GameEngine.Contract.Counters
{
    public class GenericCounter : CounterBase
    {
        private int _bonusPercentage;

        private int _currentSteps;

        private int _inflation;

        public decimal Bonus
        {
            get { return SubValue*BonusPercentage/100.0m; }
        }

        public int BonusPercentage
        {
            get { return _bonusPercentage; }
            set { _bonusPercentage = value > 0 ? value : 0; }
        }

        public int Inflation
        {
            get { return _inflation; }
            set { _inflation = value > 0 ? (value <= 100 ? value : 100) : 0; }
        }

        public int StepsToIncreaseInflation { get; set; }

        public int CurrentSteps
        {
            get { return _currentSteps; }
            set
            {
                _currentSteps = value > 0
                    ? (value <= StepsToIncreaseInflation*100 ? value : StepsToIncreaseInflation*100)
                    : 0;
            }
        }

        public override decimal Value
        {
            get
            {
                var total = SubValue + Bonus;
                return total - total*Inflation/100;
            }
        }
    }
}