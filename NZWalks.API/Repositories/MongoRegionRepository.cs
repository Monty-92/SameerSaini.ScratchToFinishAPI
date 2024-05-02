using MongoDB.Driver;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories;

public class MongoRegionRepository: IRegionRepository
{
    private readonly IMongoCollection<RegionModel> _regionsCollection;

    public MongoRegionRepository(NZWalksDbMongoLocalContext dbContext)
    {
        _regionsCollection = dbContext.Regions;
    }
    
    public async Task<List<RegionModel>> GetAllRegions()
    {
        return await _regionsCollection.Find(_ => true).ToListAsync();
    }

    public async Task<RegionModel?> GetRegionById(Guid id)
    {
        return await _regionsCollection.Find(r => r.Id == id).FirstOrDefaultAsync();
    }

    public async Task<RegionModel> CreateRegion(RegionModel region)
    {
        await _regionsCollection.InsertOneAsync(region);
        return region;
    }

    public async Task<RegionModel?> UpdateRegion(Guid id, RegionModel region)
    {
        RegionModel existingRegionDomainModel = await _regionsCollection.Find(r => r.Id == id).FirstOrDefaultAsync();

        if (existingRegionDomainModel == null)
        {
            return null;
        }

        // Map DTO to Domain Model
        existingRegionDomainModel.Code = region.Code ?? existingRegionDomainModel.Code;
        existingRegionDomainModel.Name = region.Name ?? existingRegionDomainModel.Name;
        existingRegionDomainModel.RegionImageUrl = region.RegionImageUrl ?? existingRegionDomainModel.RegionImageUrl;

        var updateResult = await _regionsCollection.ReplaceOneAsync(r => r.Id == id, existingRegionDomainModel);
        
        // if(updateResult.ModifiedCount == 0) return null;

        return existingRegionDomainModel;
    }

    public async Task<RegionModel?> DeleteRegion(Guid id)
    {
        // Get Region Domain Models From Database
            RegionModel existingRegionDomainModel = await _regionsCollection.Find(r => r.Id == id).FirstOrDefaultAsync();

            if (existingRegionDomainModel == null)
            {
                return null;
            }

            var deleteResult = await _regionsCollection.DeleteOneAsync(r => r.Id == id);

            if (deleteResult.DeletedCount == 0)
            {
                return null;
            }

            return existingRegionDomainModel;
    }
}