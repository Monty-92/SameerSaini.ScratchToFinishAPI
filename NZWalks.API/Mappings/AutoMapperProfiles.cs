using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        //Regions
        CreateMap<RegionModel, RegionDto>().ReverseMap();
        CreateMap<AddRegionRequestDto, RegionModel>().ReverseMap();
        CreateMap<UpdateRegionRequestDto, RegionModel>().ReverseMap();

        // Walks
        CreateMap<AddWalkRequestDto, Walk>().ReverseMap();
        CreateMap<Walk, WalkDto>().ReverseMap();
        CreateMap<UpdateWalkRequestDto, Walk>().ReverseMap();

        // Difficulties
        CreateMap<Difficulty, DifficultyDto>().ReverseMap();
    }
}