using System;
using Dapr.Client;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Collections.Generic;

string text = "SAM SAM";

char[] textochar = text.ToCharArray();
List<byte> text_bytes = new List<byte>();
List<byte> search_buffer = new List<byte>();

foreach (var item in textochar)
{
    var bytes = Convert.ToByte(item);
    text_bytes.Add(bytes);
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

// Publish an event/message using Dapr PubSub
object value = await DaprClient.PublishEventAsync("compresspubsub", "compress", texto_comprimido);
Console.WriteLine("Published data: " + texto_comprimido);

await Task.Delay(TimeSpan.FromSeconds(1));


public record Compress([property: JsonPropertyName("string_compress")] string texto_comprimido);
