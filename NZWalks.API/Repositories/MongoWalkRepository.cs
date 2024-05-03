using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class MongoWalkRepository : IWalkRepository
    {
        private readonly IMongoCollection<Walk> _walksCollection;
        private readonly IMongoCollection<Difficulty> _difficultiesCollection;
    private readonly IMongoCollection<RegionModel> _regionsCollection;

        public MongoWalkRepository(NZWalksDbMongoLocalContext dbContext)
        {
            _walksCollection = dbContext.Walks;
            _difficultiesCollection = dbContext.Difficulties;
            _regionsCollection = dbContext.Regions;
        }
        public async Task<Walk> CreateWalk(Walk walk)
        {
            await _walksCollection.InsertOneAsync(walk);
            return walk;
        }

        public async Task<List<Walk>> GetAllWalks()
        {
            // Create the aggregation pipeline using fluent API
            var pipeline = GeneratePipeline();
            
            return await _walksCollection
                .Aggregate<Walk>(pipeline)
                .ToListAsync();
        }

        public async Task<Walk?> GetWalkById(Guid id)
        {
            // Create the aggregation pipeline using fluent API
            var pipeline = GeneratePipeline(id);
            
            return await _walksCollection
                .Aggregate<Walk>(pipeline)
                .FirstOrDefaultAsync();
        }

        public async Task<Walk?> UpdateWalk(Guid id, Walk walk)
        {
            var updateResult = await _walksCollection.ReplaceOneAsync(w => w.Id == id, walk);

            if (updateResult == null) return null;

            // Create the aggregation pipeline using fluent API
            var pipeline = GeneratePipeline(id);
            
            return await _walksCollection
                .Aggregate(pipeline)
                .FirstOrDefaultAsync();
        }

        public async Task<Walk?> DeleteWalk(Guid id)
        {
            PipelineDefinition<Walk, Walk> pipeline = GeneratePipeline(id);

            Walk walk = await _walksCollection
                .Aggregate(pipeline)
                .FirstOrDefaultAsync();

            await _walksCollection.DeleteOneAsync(w => w.Id == id);

            Walk deletedWalk = await _walksCollection
                .Aggregate(pipeline)
                .FirstOrDefaultAsync();
            
            if (deletedWalk != null) return null;
            
            return walk;
        }

        // Generate pipeline based on optional Walk ID
        private PipelineDefinition<Walk, Walk> GeneratePipeline(Guid? walkId = null)
        {
            var pipeline = new List<BsonDocument>();

            // Lookup for Difficulty
            pipeline.Add(new BsonDocument
            {
                { "$lookup", new BsonDocument
                    {
                        { "from", "difficulties" },
                        { "localField", "DifficultyId" },
                        { "foreignField", "_id" },
                        { "as", "Difficulty" }
                    }
                }
            });

            // Lookup for Region
            pipeline.Add(new BsonDocument
            {
                { "$lookup", new BsonDocument
                    {
                        { "from", "regions" },
                        { "localField", "RegionId" },
                        { "foreignField", "_id" },
                        { "as", "Region" }
                    }
                }
            });

            // Unwind Difficulty and Region arrays
            pipeline.Add(new BsonDocument { { "$unwind", "$Difficulty" } });
            pipeline.Add(new BsonDocument { { "$unwind", "$Region" } });

            // If walk ID is specified, add a match stage
            if (walkId.HasValue)
            {
                pipeline.Add(new BsonDocument
                {
                    { "$match", new BsonDocument
                        {
                            { "_id", walkId.Value }
                        }
                    }
                });
            }

            return pipeline;
        }

        // public BsonDocument[] GeneratePipeline(Guid id = Guid.Empty)
        // {
        //     if (id != Guid.Empty)
        //     {
        //         var pipeline = new[]
        //         {
        //             // Match the walk by ID
        //             PipelineStageDefinitionBuilder.Match<Walk>(walk => walk.Id == id),

        //             // Lookup with Difficulties collection
        //             new BsonDocument
        //             {
        //                 { "$lookup", new BsonDocument
        //                     {
        //                         { "from", "difficulties" },
        //                         { "localField", "DifficultyId" },
        //                         { "foreignField", "_id" },
        //                         { "as", "Difficulty" }
        //                     }
        //                 }
        //             },
        //             // Lookup with Regions collection
        //             new BsonDocument
        //             {
        //                 { "$lookup", new BsonDocument
        //                     {
        //                         { "from", "regions" },
        //                         { "localField", "RegionId" },
        //                         { "foreignField", "_id" },
        //                         { "as", "Region" }
        //                     }
        //                 }
        //             },
        //             // Unwind the arrays to get single elements
        //             new BsonDocument { { "$unwind", "$Difficulty" } },
        //             new BsonDocument { { "$unwind", "$Region" } }
        //         };
        //     }
        //     else
        //     {
        //         var pipeline = new[]
        //         {
        //             // Lookup with Difficulties collection
        //             new BsonDocument
        //             {
        //                 { "$lookup", new BsonDocument
        //                     {
        //                         { "from", "difficulties" },
        //                         { "localField", "DifficultyId" },
        //                         { "foreignField", "_id" },
        //                         { "as", "Difficulty" }
        //                     }
        //                 }
        //             },
        //             // Lookup with Regions collection
        //             new BsonDocument
        //             {
        //                 { "$lookup", new BsonDocument
        //                     {
        //                         { "from", "regions" },
        //                         { "localField", "RegionId" },
        //                         { "foreignField", "_id" },
        //                         { "as", "Region" }
        //                     }
        //                 }
        //             },
        //             // Unwind the arrays to get single elements
        //             new BsonDocument { { "$unwind", "$Difficulty" } },
        //             new BsonDocument { { "$unwind", "$Region" } }
        //         };
        //     }

        //     return pipeline;
        // }
    }
}