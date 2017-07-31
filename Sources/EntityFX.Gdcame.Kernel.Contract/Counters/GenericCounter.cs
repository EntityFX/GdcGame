namespace EntityFX.Gdcame.Kernel.Contract.Counters
{
    public class GenericCounter : CounterBase
    {
        private int _bonusPercentage;

        private int _currentSteps;

        private int _inflation;

        public decimal Bonus
        {
            get { return this.SubValue*this.BonusPercentage/100.0m; }
        }

        public int BonusPercentage
        {
            get { return this._bonusPercentage; }
            set { this._bonusPercentage = value > 0 ? value : 0; }
        }

        public int Inflation
        {
            get { return this._inflation; }
            set { this._inflation = value > 0 ? (value <= 100 ? value : 100) : 0; }
        }

        public int StepsToIncreaseInflation { get; set; }

        public int CurrentSteps
        {
            get { return this._currentSteps; }
            set
            {
                this._currentSteps = value > 0
                    ? (value <= this.StepsToIncreaseInflation*100 ? value : this.StepsToIncreaseInflation*100)
                    : 0;
            }
        }

        public override decimal Value
        {
            get
            {
                var total = this.SubValue + this.Bonus;
                return total - total*this.Inflation/100;
            }
        }
    }
}