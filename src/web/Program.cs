var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Services.AddSingleton<TableServiceClient>(sp =>
{
  var configuration = sp.GetRequiredService<IConfiguration>();
  var connectionString = configuration.GetConnectionString("StorageAccount");
  return new TableServiceClient(connectionString);
});

app.MapGet("/", () => "Hello World!");

app.Run();
