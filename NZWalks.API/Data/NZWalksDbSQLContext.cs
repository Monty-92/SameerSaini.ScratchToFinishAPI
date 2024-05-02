// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Internal;
// using NZWalks.API.Models.Domain;
// using MongoDB.Driver;
// using MongoDB.EntityFrameworkCore.Extensions;

// namespace NZWalks.API.Data
// {
//     public class NZWalksDbSQLContext : DbContext
//     {
//         public NZWalksDbSQLContext(DbContextOptions<NZWalksDbSQLContext> dbContextOptions) : base(dbContextOptions)
//         {

//         }

//     //     protected override void OnModelCreating(ModelBuilder modelBuilder)
//     // {
//     //     base.OnModelCreating(modelBuilder);
//     //     modelBuilder.Entity<Difficulty>().ToCollection("customers");
//     //     modelBuilder.Entity<Region>().ToCollection("regions");
//     //     modelBuilder.Entity<Walk>().ToCollection("walks");
//     // }

//         public DbSet<Difficulty> Difficulties{ get; init; }
//         public DbSet<Region> Regions { get; init; }
//         public DbSet<Walk> Walks { get; init; }
//     }
// }