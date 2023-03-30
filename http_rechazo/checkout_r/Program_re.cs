var baseURL = (Environment.GetEnvironmentVariable("BASE_URL") ?? "http://localhost") + ":" + (Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "3500"); //reconfigure cpde to make requests to Dapr sidecar
const string PUBSUBNAME = "rechazopubsub";
const string TOPIC = "rechazo";
Console.WriteLine($"Publishing to baseURL: {baseURL}, Pubsub Name: {PUBSUBNAME}, Topic: {TOPIC} ");

var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

string text = "SAM SAM";
string texto_rechazado = "";

if(text.Contains("INGENIERO"))
{
  texto_rechazado = "Se rechazo la solicitud por escribir un token prohibido";
}else{
  texto_rechazado = "Si se podrá comprimir el texto enviado";
  httpClient.compresspubsub();
}

var stringJson = JsonSerializer.Serialize<Rechazo>(texto_rechazado);
var content = new StringContent(stringJson, Encoding.UTF8, "application/json");

// Publish an event/message using Dapr PubSub via HTTP Post
var response = httpClient.PostAsync($"{baseURL}/v1.0/publish/{PUBSUBNAME}/{TOPIC}", content);
Console.WriteLine("Published data: " + texto_rechazado);
await Task.Delay(TimeSpan.FromSeconds(1));

public record Rechazo([property: JsonPropertyName("string_rechazo")] string texto_rechazado);