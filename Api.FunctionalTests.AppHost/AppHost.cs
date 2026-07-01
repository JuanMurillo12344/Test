var builder = DistributedApplication.CreateBuilder(args);

var bd = builder
    .AddSqlServer("bdserver-tests")
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase("bd-tests");

builder.AddProject<Projects.Api>("api-tests")
    .WithExternalHttpEndpoints()
    .WithReference(bd)
    .WaitFor(bd);

builder.Build().Run();