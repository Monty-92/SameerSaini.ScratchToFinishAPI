// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using NZWalks.API.Models.Domain;

// namespace NZWalks.API.Repositories
// {
//     public class InMemoryRegionRepository : IRegionRepository
//     {
//         public async Task<List<Region>> GetAllRegions()
//         {
//             return new List<Region>()
//             {
//                 new Region()
//                 {
//                     Id = Guid.NewGuid(),
//                     Code = "SAM",
//                     Name = "Brian's Region Name"
//                 }
//             };
//         }
//     }
// }