using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using MongoDB.Driver;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbMongoLocalContext :DbContext
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

        public void Seed()
        {
            // Check if difficulties are already seeded
            bool anyDifficultyExists = Difficulties.Find(_ => true).Any();

            if (!anyDifficultyExists) Difficulties.InsertMany(GenerateDifficultiesList());

            // Check if regions are already seeded
            var anyRegionExists = Regions.Find(_ => true).Any();

            if (!anyRegionExists) Regions.InsertMany(GenerateRegionsList());
        }
        #region Seed Data: Difficulties
        public List<Difficulty> GenerateDifficultiesList()
        {
            // Seed data for Difficulties
            // Easy, Medium, Hard
            var difficulties = new List<Difficulty>()
            {
                new Difficulty()
                {
                    Id = Guid.Parse("3c741467-fde4-493f-9462-a31efc603eaf"),
                    Name = "Easy"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("3563c9e3-459a-4956-b2e0-01e57c1a6440"),
                    Name = "Medium"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("6206522c-fe90-44b3-9d07-f9ab13f1bba2"),
                    Name = "Hard"
                }
            };
            return difficulties;
        }
        #endregion Seed Data: Difficulties

        #region Seed Data: Regions
        public List<RegionModel> GenerateRegionsList()
        {
            // Seed Data for Regions
            var regions = new List<RegionModel>()
            {
                new RegionModel
                {
                    Id = Guid.Parse("73f1d8fa-aa2f-4ea9-9922-f6200daa2412"),
                    Name = "Auckland",
                    Code = "AKL",
                    RegionImageUrl = "demo-image-AKL.jpg"
                },
                new RegionModel
                {
                    Id = Guid.Parse("5d99ac34-7529-4063-aca9-47c537817b97"),
                    Name = "Northland",
                    Code = "NTL",
                    RegionImageUrl = "demo-image-NTL.jpg"
                },
                new RegionModel
                {
                    Id = Guid.Parse("a2e27efc-d480-492d-aaa3-29d4c6166050"),
                    Name = "Bay of Plenty",
                    Code = "BOP",
                    RegionImageUrl = "demo-image-BOP.jpg"
                },
                new RegionModel
                {
                    Id = Guid.Parse("747d6f90-d433-49d4-ac37-8fc9b06aa856"),
                    Name = "Wellington",
                    Code = "WGN",
                    RegionImageUrl = "demo-image-WGN.jpg"
                },
                new RegionModel
                {
                    Id = Guid.Parse("513883fb-e323-4487-910b-e037efe47283"),
                    Name = "Nelson",
                    Code = "NSN",
                    RegionImageUrl = "demo-image-NSN.jpg"
                },
                new RegionModel
                {
                    Id = Guid.Parse("c82eab80-680a-45c2-b6d0-ea96939954c5"),
                    Name = "Southland",
                    Code = "STL",
                    RegionImageUrl = "demo-image-STL.jpg"
                }
            };

            return regions;
        }
        #endregion Seed Data: Regions

        #region Seeding: SQL
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed difficulties to the database
            modelBuilder.Entity<Difficulty>().HasData(GenerateDifficultiesList());
            
            // Seed Regions to the database
            modelBuilder.Entity<RegionModel>().HasData(GenerateRegionsList()); 
        }
        #endregion Seed Data: SQL
    }
}

public class MongoDBLocalSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}