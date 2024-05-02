using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NZWalks.API.Models.Domain;
using NZWalks.API.Controllers;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepository
    {
        Task<List<RegionModel>> GetAllRegions();
        Task<RegionModel?> GetRegionById(Guid id);
        Task<RegionModel> CreateRegion(RegionModel region);
        Task<RegionModel?> UpdateRegion(Guid id, RegionModel region);
        Task<RegionModel?> DeleteRegion(Guid id);
    }
}