using NZWalks.API.Data;
using NZWalks.API.Mappings;
using NZWalks.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Database Manager
ConfigureMongoDB();
// builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));

// builder.Services.AddSingleton<NZWalksDbMongoLocalContext>();

// builder.Services.AddScoped<IRegionRepository, InMemoryRegionRepository>();
builder.Services.AddScoped<IRegionRepository, MongoRegionRepository>();
builder.Services.AddScoped<IWalkRepository, MongoWalkRepository>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Seed the database
var dbContext = app.Services.GetService<NZWalksDbMongoLocalContext>();
dbContext.Seed();

app.Run();






// using Microsoft.EntityFrameworkCore;
// using MongoDB.Driver;
// using NZWalks.API.Data;

// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// builder.Services.AddControllers();
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// // Configure Database Manager
// ConfigureMongoDBlocal();


// string dbMan = "Mongo";

// if (dbMan == "Mongo")
// {
//     bool useCluster = false;
//     ConfigureMongoDBlocal();
// } 
// else if (dbMan == "SQL")
// {
//     ConfigureSQL();
// }
// else
// {
//     throw new Exception("A database manager needs to be selected");
// }

// // var mongoClient = new MongoClient(builder.Configuration.GetConnectionString("NZWalksConnectionString"));
// builder.Services.AddDbContext<NZWalksDbContext>(options =>
//     options.UseMongoDB(
//         new MongoClient(builder.Configuration.GetConnectionString("NZWalksMongoConnectionString")), 
//         "NZWalksDb")
// // options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksConnectionString"))
// );

// var mongoClient = new MongoClient("<Your MongoDB Connection URI>");

// var dbContextOptions =
//     new DbContextOptionsBuilder<NZWalksDbContext>().UseMongoDB(mongoClient, "NZWalksDb");

// var db = new NZWalksDbContext(dbContextOptions.Options);

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

// app.UseAuthorization();

// app.MapControllers();

// app.Run();


// void ConfigureDatabaseManager(string dbMan, bool useCluster = false)
// {
//     // Configure Database Manager
//     if (dbMan == "Mongo")
//     {
//         if(useCluster) ConfigureMongoDBcluster();
//         // else if (!useCluster) ConfigureMongoDBlocal();
//     } 
//     else if (dbMan == "SQL")
//     {
//         // ConfigureSQL();
//     }
//     else
//     {
//         throw new Exception("A database manager needs to be selected");
//     }


void ConfigureMongoDB(bool local = true)
{
    if (local) ConfigureMongoDBlocal();
    else ConfigureMongoDBcluster();
}

void ConfigureMongoDBlocal()
{
    builder.Services.Configure<MongoDBLocalSettings>(builder.Configuration.GetSection("MongoDB"));
    builder.Services.AddSingleton<NZWalksDbMongoLocalContext>();
}
void ConfigureMongoDBcluster()
{
    builder.Services.Configure<MongoDBClusterSettings>(builder.Configuration.GetSection("MongoDB"));
    builder.Services.AddSingleton<NZWalksDbMongoClusterContext>();
}
// void ConfigureMongoDBcluster()
// {
//     builder.Services.AddDbContext<NZWalksDbMongoClusterContext>(options =>
//         options.UseMongoDB(
//             new MongoClient(builder.Configuration.GetConnectionString("NZWalksMongoClusterConnectionString")), 
//             "NZWalksDb")
//     );
// }

// void ConfigureSQL()
// {
//     builder.Services.AddDbContext<NZWalksDbSQLContext>(options =>
//         options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksSQLConnectionString"))
//     );
// }