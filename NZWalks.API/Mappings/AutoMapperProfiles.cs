using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RegionModel, RegionDto>().ReverseMap();
            CreateMap<AddRegionRequestDto, RegionModel>().ReverseMap();
            CreateMap<UpdateRegionRequestDto, RegionModel>().ReverseMap();
        }
    }
}