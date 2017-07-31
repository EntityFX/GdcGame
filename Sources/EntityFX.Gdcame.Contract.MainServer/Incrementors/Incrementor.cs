namespace EntityFX.Gdcame.Contract.MainServer.Incrementors
{
    using System.Xml.Serialization;

    [XmlRoot]
    public class Incrementor
    {
        [XmlElement]
        public IncrementorTypeEnum IncrementorType { get; set; }

        [XmlElement]
        public int Value { get; set; }
    }
}