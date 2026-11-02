var builder = DistributedApplication.CreateBuilder(args);

// PostgreSQL server
var postgresServer = builder.AddPostgres("postgres-server");

// PostgreSQL databases
var identityDb = postgresServer.AddDatabase("identitydb");

var cartDb = postgresServer.AddDatabase("cartdb");

var catalogDb = postgresServer.AddDatabase("CatalogDb");

var paymentDatabase = postgresServer.AddDatabase("paymentdb");


// SQL Server resource
var sqlServer = builder.AddSqlServer("sqlserver");

// Order database hosted on SQL Server
var orderDb = sqlServer.AddDatabase("orderDb");


// Register microservices

builder.AddProject<Projects.Payment_Api>("payment-api")
    .WithReference(paymentDatabase);

builder.AddProject<Projects.Catalog_Api>("catalog-api")
    .WithReference(catalogDb);

builder.AddProject<Projects.Identity_Api>("identity-api")
    .WithReference(identityDb)
    .WaitFor(identityDb);

builder.AddProject<Projects.Order_Api>("order-api")
    .WithReference(orderDb)
    .WaitFor(orderDb);

builder.AddProject<Projects.Cart_Api>("cart-api")
    .WithReference(cartDb)
    .WaitFor(cartDb);


builder.Build().Run();