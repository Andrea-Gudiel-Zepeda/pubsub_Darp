using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

if (app.Environment.IsDevelopment()) {app.UseDeveloperExceptionPage();}

// Register Dapr pub/sub subscriptions
app.MapGet("/dapr/subscribe", () => {
    var sub = new DaprSubscription(PubsubName: "rechazopubsub", Topic: "rechazo", Route: "rechazo");
    Console.WriteLine("Dapr pub/sub is subscribed to: " + sub);
    return Results.Json(new DaprSubscription[]{sub});
});

// Dapr subscription in /dapr/subscribe sets up this route
app.MapPost("/rechazo", (DaprData<Rechazo> requestData) => {
    Console.WriteLine("Subscriber received : " + requestData.Data.string_rechazo);
    return Results.Ok(requestData.Data);
});

await app.RunAsync();

public record DaprData<T> ([property: JsonPropertyName("data")] T Data); 
public record Rechazo([property: JsonPropertyName("string_rechazo")] string string_rechazo);
public record DaprSubscription(
  [property: JsonPropertyName("pubsubname")] string PubsubName, 
  [property: JsonPropertyName("topic")] string Topic, 
  [property: JsonPropertyName("route")] string Route);
