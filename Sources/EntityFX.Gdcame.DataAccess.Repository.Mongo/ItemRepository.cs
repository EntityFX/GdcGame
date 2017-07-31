namespace EntityFX.Gdcame.DataAccess.Repository.Mongo.MainServer
{
    using EntityFX.Gdcame.Common.Contract.Incrementors;
    using EntityFX.Gdcame.Common.Contract.Items;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.FundsDriver;

    using MongoDB.Bson;
    using MongoDB.Driver;

    public class ItemRepository : IItemRepository
    {
        private IMongoDatabase Database
        {
            get; set;
        }

        public ItemRepository(IMongoDatabase database)
        {
            this.Database = database;
        }

        public Item[] FindAll(GetAllFundsDriversCriterion criterion)
        {
            IMongoCollection<Item> itemCollection = this.Database.GetCollection<Item>("Item");

            var filter = new BsonDocument();
            if (itemCollection.Find<Item>(filter).Count() == 0)
            {
                Item[] items = new Item[] {
                    new Item() {
                    Id = 1, Name = "Item", Price = 300, IsUnlocked = true, InitialValue = 100
                        , Incrementors = new Incrementor[] {
                            new Incrementor() { Value = 1, IncrementorType = IncrementorTypeEnum.ValueIncrementor },
                            new Incrementor() { Value = 10, IncrementorType = IncrementorTypeEnum.ValueIncrementor },
                            new Incrementor() { Value = 0, IncrementorType = IncrementorTypeEnum.ValueIncrementor }
                         }
                    }
                };
                itemCollection.InsertMany(items);
            }

            return itemCollection.Find(filter).SortBy(_ => _.Id).ToList().ToArray();
        }
    }
}
