using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data;

public class NZWalksDbMongoClusterContext
{
    private readonly IMongoDatabase _database;

    public NZWalksDbMongoClusterContext(IOptions<MongoDBClusterSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    // Define methods to work with collections here
    // For example:
    public IMongoCollection<Walk> Walks => _database.GetCollection<Walk>("walks");
    public IMongoCollection<Difficulty> Difficulties => _database.GetCollection<Difficulty>("difficulties");
    public IMongoCollection<RegionModel> Regions => _database.GetCollection<RegionModel>("regions");
}

public class MongoDBClusterSettings
{
public string ConnectionString { get; set; }
public string DatabaseName { get; set; }
}