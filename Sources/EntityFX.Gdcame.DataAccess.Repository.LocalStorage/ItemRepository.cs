using EntityFX.Gdcame.DataAccess.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Common.Contract.Items;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.FundsDriver;
using EntityFX.Gdcame.Common.Contract.Incrementors;
using System.IO;
using Newtonsoft.Json;

namespace EntityFX.Gdcame.DataAccess.Repository.LocalStorage
{
    public class ItemRepository : IItemRepository
    {
        public Item[] FindAll(GetAllFundsDriversCriterion criterion)
        {
            if (!File.Exists(Path.Combine("storage", "items", "items.json")))
            {
                return null;
            }
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(Path.Combine("storage", "items", "items.json")))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.TypeNameHandling = TypeNameHandling.Auto;
                return (Item[])serializer.Deserialize(file, typeof(Item[]));
            }
        }
    }
}
