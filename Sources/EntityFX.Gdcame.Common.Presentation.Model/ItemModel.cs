using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml.Serialization;
using EntityFX.Gdcame.Common.Contract.Incrementors;

namespace EntityFX.Gdcame.Common.Presentation.Model
{
    [XmlType("Item")]
    public class ItemModel
    {
        [XmlElement]
        public int Id { get; set; }
        [XmlElement]
        public string Name { get; set; }
        [XmlElement]
        public decimal Value { get; set; }
        [XmlElement]
        public int InflationPercent { get; set; }
        [XmlElement]
        public decimal UnlockValue { get; set; }
        [XmlElement]
        public bool IsActive { get; set; }
        [XmlElement]
        public int BuyCount { get; set; }
        [XmlElement(IsNullable = true)]
        public string Picture { get; set; }
        [XmlElement]
        [XmlArray("Incrementors")]
        [XmlArrayItem("Pair")]
        public IDictionary<int, Incrementor> Incrementors { get; set; }
    }
}