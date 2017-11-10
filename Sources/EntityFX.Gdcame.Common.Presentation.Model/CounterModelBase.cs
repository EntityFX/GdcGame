using System;


namespace EntityFX.Gdcame.Common.Application.Model
{
    //
    //[KnownType(typeof(GenericCounterModel))]
    //[KnownType(typeof(SingleCounterModel))]
    //[KnownType(typeof(DelayedCounterModel))]
    public class CounterModelBase
    {
        //
        public int Id { get; set; }
        //
        public string Name { get; set; }
        //
        public decimal Value { get; set; }
        //
        public int Type { get; set; }
    }


}