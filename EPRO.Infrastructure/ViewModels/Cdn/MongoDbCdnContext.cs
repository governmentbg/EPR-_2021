using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace EPRO.Infrastructure.ViewModels.Cdn
{
    public abstract class MongoDbCdnContext
    {
        public IGridFSBucket GridFsBucket { get; }
        public MongoClient Client { get; }
        protected MongoDbCdnContext(IConfiguration config)
        {
            var connectionString = config.GetSection("Settings").GetValue<string>("mongodb");
            var connection = new MongoUrl(connectionString);
            var settings = MongoClientSettings.FromUrl(connection);
            
            Client = new MongoClient(settings);            
            var database = Client.GetDatabase(connection.DatabaseName);
            GridFsBucket = new GridFSBucket(database);
        }
    }
}
