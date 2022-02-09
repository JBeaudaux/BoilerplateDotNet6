using BoilerplateDotNet6.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace BoilerplateDotNet6.Models
{
    public class DBAccessService
    {
        // private readonly IMongoCollection<MyModel> _myCollection;

        public DBAccessService(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            // _myCollection = database.GetCollection<MyModel>("CollectionName");
        }

        // ----------------------- Write access functions here ----------------------- //

    }
}