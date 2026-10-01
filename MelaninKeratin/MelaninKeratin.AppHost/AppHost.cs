//var builder = DistributedApplication.CreateBuilder(args);

//var apiService = builder.AddProject<Projects.MelaninKeratin_ApiService>("apiservice")
//    .WithHttpHealthCheck("/health");

//builder.AddProject<Projects.MelaninKeratin_Web>("webfrontend")
//    .WithExternalHttpEndpoints()
//    .WithHttpHealthCheck("/health")
//    .WithReference(apiService)
//    .WaitFor(apiService);

//builder.Build().Run();

var builder = DistributedApplication.CreateBuilder(args);

// 1. Tell Aspire to run a Docker container for PostgreSQL
var postgresServer = builder.AddPostgres("postgres-server");

// 2. Create your individual database on that server instance
var catalogDb = postgresServer.AddDatabase("CatalogDb");

// 3. Pass that specific database connection into your API project
builder.AddProject<Projects.MelaninKeratin_ApiService>("catalog-api")
       .WithReference(catalogDb); // Aspire securely injects this string at runtime

builder.Build().Run();