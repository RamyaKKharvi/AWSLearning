using Amazon.SQS;
using AWS_EC_WorkerService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

// Note: Add AWS configurations
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());

// Note: Register services
builder.Services.AddAWSService<IAmazonSQS>();
builder.Services.AddLogging(configure =>
{
    configure.AddAWSProvider(builder.Configuration.GetAWSLoggingConfigSection());
});
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
