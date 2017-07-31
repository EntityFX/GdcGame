namespace EntityFX.Gdcame.DataAccess.Repository.LocalStorage.MainServer
{
    using System.IO;

    using EntityFX.Gdcame.Contract.MainServer.Items;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.FundsDriver;

    using Newtonsoft.Json;

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
