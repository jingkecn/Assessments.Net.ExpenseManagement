using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var db = builder
    .AddSqlServer("database")
    .WithDataVolume();

builder
    .AddProject<Command_Api>("command-api")
    .WithReference(db);

builder
    .AddProject<Query_Api>("query-api")
    .WithReference(db);

builder.Build().Run();
