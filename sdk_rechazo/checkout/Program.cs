using System;
using Dapr.Client;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Collections.Generic;

string text = "SAM SAM";
string texto_rechazado = "";

if(text.Contains("INGENIERO"))
{
  texto_rechazado = "Se rechazo la solicitud por escribir un token prohibido";
}else{
  texto_rechazado = "Si se podrá comprimir el texto enviado";
  httpClient.compresspubsub();
}


// Publish an event/message using Dapr PubSub
object value = await DaprClient.PublishEventAsync("rechazadopubsub", "rechazado", texto_rechazado);
Console.WriteLine("Published data: " + texto_rechazado);

await Task.Delay(TimeSpan.FromSeconds(1));


public record Rechazo([property: JsonPropertyName("string_rechazo")] string texto_rechazado);
