var builder = DistributedApplication.CreateBuilder(args);


// 1. Tell Aspire to run a Docker container for PostgreSQL
var postgresServer = builder.AddPostgres("postgres-server");

var identityDb = postgresServer.AddDatabase("identitydb");

var cartDb = postgresServer.AddDatabase("cartdb");

// 2. Create your individual database on that server instance
var catalogDb = postgresServer.AddDatabase("CatalogDb");

var orderdb = postgresServer.AddDatabase("orderDb");

var paymentDatabase = postgresServer.AddDatabase("paymentdb");

builder.AddProject<Projects.Payment_Api>("payment-api").WithReference(paymentDatabase);

// 3. Pass that specific database connection into your API project
builder.AddProject<Projects.Catalog_Api>("catalog-api").WithReference(catalogDb); // Aspire securely injects this string at runtime

builder.AddProject<Projects.Identity_Api>("identity-api").WithReference(identityDb).WaitFor(identityDb);

builder.AddProject<Projects.Order_Api>("order-api").WithReference(orderdb).WaitFor(orderdb);

builder.AddProject<Projects.Cart_Api>("cart-api").WithReference(cartDb).WaitFor(cartDb);

builder.Build().Run();