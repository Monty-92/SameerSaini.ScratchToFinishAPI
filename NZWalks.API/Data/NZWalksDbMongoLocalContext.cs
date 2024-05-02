using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbMongoLocalContext
    {
        private readonly IMongoDatabase _database;

        public NZWalksDbMongoLocalContext(IOptions<MongoDBLocalSettings> settings)
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
}

public class MongoDBLocalSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}


// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Internal;
// using NZWalks.API.Models.Domain;
// using MongoDB.Driver;
// using MongoDB.EntityFrameworkCore.Extensions;

// namespace NZWalks.API.Data
// {
//     public class NZWalksDbMongoLocalContext : DbContext
//     {
//         public NZWalksDbMongoLocalContext(DbContextOptions<NZWalksDbMongoLocalContext> dbContextOptions) : base(dbContextOptions)
//         {
 
//         }

//         protected override void OnModelCreating(ModelBuilder modelBuilder)
//     {
//         base.OnModelCreating(modelBuilder);
//         modelBuilder.Entity<Difficulty>().ToCollection("customers");
//         modelBuilder.Entity<Region>().ToCollection("regions");
//         modelBuilder.Entity<Walk>().ToCollection("walks");
//     }

//         public DbSet<Difficulty> Difficulties{ get; init; }
//         public DbSet<Region> Regions { get; init; }
//         public DbSet<Walk> Walks { get; init; }
//     }
// }