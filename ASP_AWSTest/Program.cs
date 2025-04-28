using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using Amazon.SageMakerRuntime;
using Amazon.SimpleEmail;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using ASP_AWSTest.IService;
using ASP_AWSTest.Service;
using AWS.Logger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

#region
// Note: Added AWS Config
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());

// Add AWS services
builder.Services.AddAWSService<IAmazonSimpleEmailService>();
builder.Services.AddAWSService<IAmazonSimpleNotificationService>();
builder.Services.AddAWSService<IAmazonSQS>();
builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddAWSService<IAmazonSageMakerRuntime>();

// Add your services
builder.Services.AddTransient<ISESService, SESService>();
builder.Services.AddTransient<ISNSService, SNSService>();
builder.Services.AddTransient<ISQSService, SQSService>();
builder.Services.AddTransient<IDynamoDbService, DynamoDbService>();
builder.Services.AddTransient<IS3Service, S3Service>();
builder.Services.AddTransient<ISageMakerService, SageMakerService>();

var awsOptions = builder.Configuration.GetSection("AWS").Get<AWSLoggerConfig>();
builder.Logging.ClearProviders(); // Remove default providers
builder.Logging.AddAWSProvider(awsOptions);

#endregion

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();
