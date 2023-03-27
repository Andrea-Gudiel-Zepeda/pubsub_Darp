using System.Text.Json.Serialization;
using Dapr;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// Dapr will send serialized event object vs. being raw CloudEvent
app.UseCloudEvents();

// needed for Dapr pub/sub routing
app.MapSubscribeHandler();

if (app.Environment.IsDevelopment()) {app.UseDeveloperExceptionPage();}

// Dapr subscription in [Topic] routes orders topic to this route
app.MapPost("/compress", [Topic("compresspubsub", "compress")] (Compress string_compress) => {
    Console.WriteLine("Subscriber received : " + string_compress);
    return Results.Ok(string_compress);
});

await app.RunAsync();

public record Compress([property: JsonPropertyName("String_compress")] string string_compress);
