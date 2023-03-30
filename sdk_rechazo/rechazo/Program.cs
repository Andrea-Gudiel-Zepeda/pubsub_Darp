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
app.MapPost("/rechazo", [Topic("rechazopubsub", "rechazo")] (Rechazo string_rechazo) => {
    Console.WriteLine("Subscriber received : " + string_rechazo);
    return Results.Ok(string_rechazo);
});

await app.RunAsync();

public record Rechazo([property: JsonPropertyName("string_rechazo")] string string_rechazo);
