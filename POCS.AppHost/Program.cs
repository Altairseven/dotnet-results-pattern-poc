var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.POC_ResultsPattern>("poc-resultspattern");

builder.Build().Run();
