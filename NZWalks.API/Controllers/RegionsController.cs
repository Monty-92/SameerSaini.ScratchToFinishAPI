using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NZWalks.API.CustomActionFilters;
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
        private readonly IMapper _mapper;

        public RegionsController(NZWalksDbMongoLocalContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            _regionsCollection = dbContext.Regions;
            _regionRepository = regionRepository;
            _mapper = mapper;
        }

        // GET: ALL REGIONS
        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            // Connect to Database: Get All Domain Model Records
            List<RegionModel> regionsDomain = await _regionRepository.GetAllRegions();

            // Map Domian Models to DTOs and Return DTOs
            return Ok(_mapper.Map<List<RegionDto>>(regionsDomain));
        }

        // GET: a SPECIFIC REGION by ID
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            // Connect to Database: Get Region Domain Model From Database based on Id
            RegionModel? regionDomainModel = await _regionRepository.GetRegionById(id);

            if (regionDomainModel == null) return NotFound();

            // ap/ Convert Region Domain Model to Region DTO and Return DTO
            return Ok(_mapper.Map<RegionDto>(regionDomainModel));
        }

        // POST: Create a NEW REGION
        [HttpPost]
        [ValidateModelAttribute]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            if (addRegionRequestDto == null) return BadRequest("Region cannot be null");

            // Map/ Convert DTO to Domain Model
            RegionModel regionDomainModel = _mapper.Map<RegionModel>(addRegionRequestDto);

            // Connect to Database: Create a NEW Region Model Record
            regionDomainModel = await _regionRepository.CreateRegion(regionDomainModel);

            // Map Domain Model to DTO
            RegionDto regionsDto = _mapper.Map<RegionDto>(regionDomainModel);

            return CreatedAtAction(nameof(GetRegionById), new { id = regionsDto.Id }, regionsDto);
        }

        // PUT: UPDATE an existing REGION
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModelAttribute]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            if (updateRegionRequestDto == null) return BadRequest("No region data submitted");
            
            // Map/ Convert DTO to Domain Model
            RegionModel? regionDomainModel = _mapper.Map<RegionModel>(updateRegionRequestDto);

            // Connect to Database: Update Existing Record
            regionDomainModel = await _regionRepository.UpdateRegion(id, regionDomainModel);

            // Check if Region Exists
            if (regionDomainModel == null) return NotFound();
            
            // Convert Domain Model to DTO and Return DTO
            return Ok(_mapper.Map<RegionDto>(regionDomainModel));
        }

        // Delete a region
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            // Connect to Database: Remove Record based on Id
            var regionDomainModel = await _regionRepository.DeleteRegion(id);

            if (regionDomainModel == null) return NotFound();

            // Convert Domain Model to DTO and Return Dto
            return Ok(_mapper.Map<RegionDto>(regionDomainModel));
        }
    }
}
