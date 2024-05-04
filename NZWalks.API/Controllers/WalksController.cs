using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WalksController : ControllerBase
{
    private readonly IMongoCollection<Walk> _walksCollection;
    private readonly IMongoCollection<Difficulty> _difficultiesCollection;
    private readonly IMongoCollection<RegionModel> _regionsCollection;
    private readonly IWalkRepository _walkRepository;
    private readonly IMapper _mapper;

    public WalksController(NZWalksDbMongoLocalContext dbContext, IWalkRepository walkRepository, IMapper mapper)
    {
        _walksCollection = dbContext.Walks;
        _difficultiesCollection = dbContext.Difficulties;
        _regionsCollection = dbContext.Regions;
        _walkRepository = walkRepository;
        _mapper = mapper;
    }

    // Get all walks
    [HttpGet]
    public async Task<IActionResult> GetAllWalks(
        [FromQuery] string? filterOn, [FromQuery] string? filterQuery, 
        [FromQuery] string? sortBy, [FromQuery] bool? isAscending)
    {
        List<Walk> walksDomainModel = await _walkRepository.GetAllWalks(filterOn, filterQuery, sortBy, isAscending ?? true);
        
        if(walksDomainModel.Count == 0 || walksDomainModel == null) return NotFound();

        // Map Domain Model to DTO
        return Ok(_mapper.Map<List<WalkDto>>(walksDomainModel));
    }

    // Get a specific walk by ID
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetWalkById([FromRoute] Guid id)
    {
        Walk? walkDomainModel = await _walkRepository.GetWalkById(id);

        if (walkDomainModel == null) return NotFound();

        return Ok(_mapper.Map<WalkDto>(walkDomainModel));
    }

    // Create a new walk
    [HttpPost]
    [ValidateModelAttribute]
    public async Task<IActionResult> CreateWalk([FromBody] AddWalkRequestDto newWalk)
    {
        // Map DTO to Domain Model
        Walk walkDomainModel = _mapper.Map<Walk>(newWalk);

        // Check if difficulty and region exist
        var difficulty = await _difficultiesCollection.Find(d => d.Id == newWalk.DifficultyId).FirstOrDefaultAsync();
        var region = await _regionsCollection.Find(r => r.Id == newWalk.RegionId).FirstOrDefaultAsync();

        if (difficulty == null) return BadRequest("Invalid Difficulty ID");

        if (region == null) return BadRequest("Invalid Region ID");

        await _walkRepository.CreateWalk(walkDomainModel);

        // MapDomain Model to DTO
        return Ok(_mapper.Map<WalkDto>(walkDomainModel));
    }

    // Update an existing walk
    [HttpPut]
    [Route("{id:guid}")]
    [ValidateModelAttribute]
    public async Task<IActionResult> UpdateWalk(Guid id, [FromBody] UpdateWalkRequestDto updatedWalkDto)
    {
        if (updatedWalkDto == null) return BadRequest("Invalid walk data");

        // Map Dto to Domain Model
        var walkDomainModel = _mapper.Map<Walk>(updatedWalkDto);

        // Check if difficulty and region exist
        var difficulty = await _difficultiesCollection.Find(d => d.Id == walkDomainModel.DifficultyId).FirstOrDefaultAsync();
        var region = await _regionsCollection.Find(r => r.Id == walkDomainModel.RegionId).FirstOrDefaultAsync();

        if (difficulty == null) return BadRequest("Invalid Difficulty ID");

        if (region == null) return BadRequest("Invalid Region ID");

        walkDomainModel = await _walkRepository.UpdateWalk(id, walkDomainModel);;

        if (walkDomainModel == null) return NotFound();

        return Ok(_mapper.Map<WalkDto>(walkDomainModel));
    }

    // Delete a walk
    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteWalk(Guid id)
    {
        var walkDomainModel = await _walkRepository.DeleteWalk(id);

        if (walkDomainModel == null) return NotFound();

        return Ok(_mapper.Map<WalkDto>(walkDomainModel));
    }
}
