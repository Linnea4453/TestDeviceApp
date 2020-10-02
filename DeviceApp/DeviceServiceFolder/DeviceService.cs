using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace DeviceApp


{
    public static class DeviceService
    {
        private static DeviceClient deviceClient = DeviceClient.CreateFromConnectionString("HostName=linneaec-iothub.azure-devices.net;DeviceId=DeviceApp;SharedAccessKey=YXnFsEHdhHfAH55Z1D1OIRD+ehYOyL13MoK5HbVQAkQ=", TransportType.Mqtt);
        private static int telemetrytInterval = 5;
        private static Random rnd = new Random();





        public static Task<MethodResponse> SetTelementryInterval(MethodRequest request, object userContext) // tar emot funktionen som sätter telementryintervalen
        {
            var payload = Encoding.UTF8.GetString(request.Data).Replace("\"","");

            if (Int32.TryParse(payload, out telemetrytInterval)) // out gör att vi kan sätta en redan befintlig variabel som ligger utanför funktionen.
            {
                string json = "{\"result\": \" Exucuted direct method: " + request.Name + "\"  }"; //Json meddelandet som skapas, bygger ihop själv

                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(json), 200));
            }
            else
            {
                string json = "{\"result\": \" Invalid method: " + request.Name + "\"  }"; //Json meddelandet som skapas, bygger ihop själv
                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(json), 501));
            }

        }

        

    }
}
