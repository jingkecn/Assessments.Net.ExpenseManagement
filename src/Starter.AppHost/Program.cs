using Projects;

var builder = DistributedApplication.CreateBuilder(args);

const string prefix = "expense-management";

var password = builder
    .AddParameter($"{prefix}-mssql-password", "$tr0ngPa$$w0rd", secret: true);

var db = builder
    .AddSqlServer($"{prefix}-mssql", password, 44000)
    .WithDataVolume()
    .AddDatabase($"{prefix}-db");

builder
    .AddProject<Command_Api>($"{prefix}-command-api")
    .WithReference(db)
    .WaitFor(db);

builder
    .AddProject<Query_Api>($"{prefix}-query-api")
    .WithReference(db)
    .WaitFor(db);

builder.Build().Run();
