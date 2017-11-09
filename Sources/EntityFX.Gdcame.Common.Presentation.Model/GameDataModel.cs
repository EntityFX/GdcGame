using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace EntityFX.Gdcame.Common.Application.Model
{
    //[XmlRoot(ElementName = "GameData")]
    public class GameDataModel
    {
        //[XmlElement]
        //[XmlArray]
        //[XmlArrayItem(ElementName = "Item")]
        public ItemModel[] Items { get; set; }
        //[XmlElement]
        public CashModel Cash { get; set; }
    }
}