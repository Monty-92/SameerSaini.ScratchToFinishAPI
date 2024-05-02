using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegionsController : ControllerBase
    {
        private readonly IMongoCollection<RegionModel> _regionsCollection;
        private readonly IRegionRepository _regionRepository;

        public RegionsController(NZWalksDbMongoLocalContext dbContext, IRegionRepository regionRepository)
        {
            _regionsCollection = dbContext.Regions;
            _regionRepository = regionRepository;
        }

        // GET: ALL REGIONS
        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            // Get Data From Database - Domain models
            // List<Region> regionsDomain = await _regionsCollection.Find(_ => true).ToListAsync();
            List<RegionModel> regionsDomain = await _regionRepository.GetAllRegions();
            // Map Domian Models to DTOs
            var regionsDto = new List<RegionDto>();
            foreach(var regionDomainModel in regionsDomain)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = regionDomainModel.Id,
                    Code = regionDomainModel.Code,
                    Name = regionDomainModel.Name,
                    RegionImageUrl = regionDomainModel.RegionImageUrl
                });
            }

            // Return DTOs
            return Ok(regionsDto);
        }

        // GET: a SPECIFIC REGION by ID
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            // Get Region Domain Models From Database
            RegionModel? regionDomainModel = await _regionRepository.GetRegionById(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Map/ Convert Region Domain Model to Region DTO
            var regionDto = new RegionDto()
                {
                    Id = regionDomainModel.Id,
                    Code = regionDomainModel.Code,
                    Name = regionDomainModel.Name,
                    RegionImageUrl = regionDomainModel.RegionImageUrl
                };

            // Return DTO
            return Ok(regionDto);
        }

        // POST: Create a NEW REGION
        [HttpPost]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            if (addRegionRequestDto == null)
            {
                return BadRequest("Region cannot be null");
            }

            // Map/ Convert DTO to Domain Model
            var regionDomainModel = new RegionModel
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            regionDomainModel = await _regionRepository.CreateRegion(regionDomainModel);
            // await _regionsCollection.InsertOneAsync(regionDomainModel);

            var regionDto = new RegionDto()
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetRegionById), new { id = regionDto.Id }, regionDto);
        }

        // PUT: UPDATE an existing REGION
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            if (updateRegionRequestDto == null)
            //  || id != updatedRegion.Id)
            {
                return BadRequest("No region data submitted");
            }
            // Map/ Convert DTO to Domain Model
            RegionModel? regionDomainModel = new RegionModel
            {
                Code = updateRegionRequestDto.Code,
                Name = updateRegionRequestDto.Name,
                RegionImageUrl = updateRegionRequestDto.RegionImageUrl
            };

            regionDomainModel = await _regionRepository.UpdateRegion(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            var updateResult = await _regionsCollection.ReplaceOneAsync(r => r.Id == id, regionDomainModel);

            // if(updateResult.ModifiedCount == 0) return NotFound();
             
            // Convert Domain Model to DTO
            var regionDto = new RegionDto()
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }

        // Delete a region
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            // Get Region Domain Models From Database
            // Region regionDomainModel = await _regionsCollection.Find(r => r.Id == id).FirstOrDefaultAsync();
            var regionDomainModel = await _regionRepository.DeleteRegion(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Convert Domain Model to DTO
            var regionDto = new RegionDto()
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }
    }
}
