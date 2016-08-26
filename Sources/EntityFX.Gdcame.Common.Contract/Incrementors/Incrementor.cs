using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace EntityFX.Gdcame.Common.Contract.Incrementors
{
    [XmlRoot]
    public class Incrementor
    {
        [XmlElement]
        public IncrementorTypeEnum IncrementorType { get; set; }

        [XmlElement]
        public int Value { get; set; }
    }
}