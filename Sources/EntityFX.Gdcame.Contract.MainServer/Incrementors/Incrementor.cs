namespace EntityFX.Gdcame.Contract.MainServer.Incrementors
{
    using System.Xml.Serialization;

    
    public class Incrementor
    {
        
        public IncrementorTypeEnum IncrementorType { get; set; }

        
        public int Value { get; set; }
    }
}