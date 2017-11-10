
using System.Xml.Serialization;

namespace EntityFX.Gdcame.Common.Application.Model
{
    //[XmlRoot(ElementName = "GameData")]
    public class GameDataModel
    {
        //
        //[XmlArray]
        //[XmlArrayItem(ElementName = "Item")]
        public ItemModel[] Items { get; set; }
        //
        public CashModel Cash { get; set; }
    }
}