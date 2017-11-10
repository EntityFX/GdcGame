using System.Collections.Generic;

//
using System.Xml.Serialization;

namespace EntityFX.Gdcame.Common.Application.Model
{
    //[XmlType("Item")]
    public class ItemModel
    {
        //
        public int Id { get; set; }
        //
        public string Name { get; set; }
        //
        public decimal Price { get; set; }
        //
        public int InflationPercent { get; set; }
        //
        public decimal UnlockBalance { get; set; }
        //
        public bool IsUnlocked { get; set; }
        //
        public int Bought { get; set; }
        //[XmlElement(IsNullable = true)]
        public string Picture { get; set; }
        //
        //[XmlArray("Incrementors")]
        //[XmlArrayItem("Pair")]
        public IDictionary<int, string> Incrementors { get; set; }
    }
}