var baseURL = (Environment.GetEnvironmentVariable("BASE_URL") ?? "http://localhost") + ":" + (Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "3500"); //reconfigure cpde to make requests to Dapr sidecar
const string PUBSUBNAME = "compresspubsub";
const string TOPIC = "compress";
Console.WriteLine($"Publishing to baseURL: {baseURL}, Pubsub Name: {PUBSUBNAME}, Topic: {TOPIC} ");

var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

string text = "SAM SAM";

char[] textochar = text.ToCharArray();
List<byte> text_bytes = new List<byte>();
List<byte> search_buffer = new List<byte>();

foreach (var item in textochar)
{
    var bytes = convert.ToByte(item);
    text_bytes.Add(item);
} 

string texto_comprimido = "";
int index = 0;
int offer = 0;
int Length = 1;
for(int i = 0; i<text_bytes.Count; i++)
{
    if(search_buffer != null)
    {
       bool encontrado = false;
       for(int j = 0; j<search_buffer.Count; j++)
       {
          if(search_buffer[j] == text_bytes[i])
          {
            index = j + 1;
            offer = i - index;
            encontrado = true;
            string cadena_agregar = "<" + offer + Length + ">";
            texto_comprimido = texto_comprimido + cadena_agregar;
          }
       }

       if(!encontrado)
       {
         search_buffer.Add(text_bytes[i]);
         texto_comprimido = texto_comprimido + text_bytes[i];
       }

    }else
    {
        search_buffer.Add(text_bytes[i]);
        texto_comprimido = texto_comprimido + text_bytes[i];
    }

}

var stringJson = JsonSerializer.Serialize<Compres>(texto_comprimido);
var content = new StringContent(stringJson, Encoding.UTF8, "application/json");

// Publish an event/message using Dapr PubSub via HTTP Post
var response = httpClient.PostAsync($"{baseURL}/v1.0/publish/{PUBSUBNAME}/{TOPIC}", content);
Console.WriteLine("Published data: " + texto_comprimido);
await Task.Delay(TimeSpan.FromSeconds(1));

public record Compress([property: JsonPropertyName("string_compress")] string texto_comprimido);