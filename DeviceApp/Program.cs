using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace DeviceApp
        { class Program 
        {
        private static DeviceClient deviceClient = DeviceClient.CreateFromConnectionString("HostName=linneaec-iothub.azure-devices.net;DeviceId=DeviceApp;SharedAccessKey=YXnFsEHdhHfAH55Z1D1OIRD+ehYOyL13MoK5HbVQAkQ=", TransportType.Mqtt);
        private static int telemetryInterval = 5;
        private static Random rnd = new Random();
      

       static void Main(string[] args)
        {
           deviceClient.SetMethodHandlerAsync("SetTelemetryInterval", SetTelemetryInterval, null).Wait();
            SendMessageAsync().GetAwaiter();

            Console.ReadKey();
          
        }

        private static Task<MethodResponse> SetTelemetryInterval(MethodRequest request, object userContext) // tar emot funktionen som sätter telementryintervalen
        {
            var payload = Encoding.UTF8.GetString(request.Data).Replace("\"","");

            if (Int32.TryParse(payload, out telemetryInterval)) // out gör att vi kan sätta en redan befintlig variabel som ligger utanför funktionen.
            {
                Console.WriteLine($"Interval set to: {telemetryInterval} seconds");

                string json = "{\"result\": \" Exucuted direct method: " + request.Name + "\"  }"; //Json meddelandet som skapas, bygger ihop själv

                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(json), 200));
            }
            else
            {
                string json = "{\"result\": \" Invalid method: " + request.Name + "\"  }"; //Json meddelandet som skapas, bygger ihop själv
                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(json), 501));
            }

        }

        private static async Task SendMessageAsync()
        {
            while(true)
            {
                double temp = 10 + rnd.NextDouble() * 15;
                double hum = 40 + rnd.NextDouble() * 20;

                var data = new
                {
                    temperature = temp,
                    humidity = hum
                };

                var json = JsonConvert.SerializeObject(data);
                var payload = new Message(Encoding.UTF8.GetBytes(json));
                payload.Properties.Add("temperatureAlert", (temp > 30) ? "true" : "false");

                await deviceClient.SendEventAsync(payload);
                Console.WriteLine($"Message sent: {json}");

                await Task.Delay(telemetryInterval * 1000);
            }
           

        }
    }
}
