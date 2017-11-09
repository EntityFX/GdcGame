using System.Collections.Generic;
using System.Runtime.Serialization;
//using System.ServiceModel;
using System.Xml.Serialization;

namespace EntityFX.Gdcame.Common.Application.Model
{
    //[XmlType("Item")]
    public class ItemModel
    {
        //[XmlElement]
        public int Id { get; set; }
        //[XmlElement]
        public string Name { get; set; }
        //[XmlElement]
        public decimal Price { get; set; }
        //[XmlElement]
        public int InflationPercent { get; set; }
        //[XmlElement]
        public decimal UnlockBalance { get; set; }
        //[XmlElement]
        public bool IsUnlocked { get; set; }
        //[XmlElement]
        public int Bought { get; set; }
        //[XmlElement(IsNullable = true)]
        public string Picture { get; set; }
        //[XmlElement]
        //[XmlArray("Incrementors")]
        //[XmlArrayItem("Pair")]
        public IDictionary<int, string> Incrementors { get; set; }
    }
}