using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using Xunit;


namespace DeviceAppTest
{

    public class DeviceAppTests
    {

        private static DeviceClient deviceClient = DeviceClient.CreateFromConnectionString("HostName=linneaec-iothub.azure-devices.net;DeviceId=DeviceApp;SharedAccessKey=YXnFsEHdhHfAH55Z1D1OIRD+ehYOyL13MoK5HbVQAkQ=", TransportType.Mqtt);
        private static int telemetrytInterval = 5;
        private static Random rnd = new Random();

        [Theory]
        [InlineData("SetTelementryInterval", "10", 200)]
        [InlineData("SetInterval", "10", 501)]
        public void SetTelementryInterval_SholudSetTelemetryInterval(string methodname, string payload, int statuscode) //Funktionens namn
        {
            deviceClient.SetMethodHandlerAsync(methodname, SetTelementryInterval, null).Wait();


            /*Arrange - F�rbered
            var calculator = new Calculator();
            var expected = 5;

            //Act - utf�r
            var actual = Calculator.add(2, 3);


           //Assert - j�mf�r
            assert.equal(Expected, actual    );
            */
        }

        public Task<MethodResponse> SetTelementryInterval(MethodRequest request, object userContext) // tar emot funktionen som s�tter telementryintervalen
        {
            var payload = Encoding.UTF8.GetString(request.Data);

            if (Int32.TryParse(payload, out telemetrytInterval)) // out g�r att vi kan s�tta en redan befintlig variabel som ligger utanf�r funktionen.
            {
                string json = "{\"result\": \" Exucuted direct method: " + request.Name + "\"  }"; //Json meddelandet som skapas, bygger ihop sj�lv

                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(json), 200));
            }
            else
            {
                string json = "{\"result\": \" Invalid method: " + request.Name + "\"  }"; //Json meddelandet som skapas, bygger ihop sj�lv
                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(json), 501));
            }

        }

      
        //  [Theory]
        //  [InlineData("temperatureAlert", "message")]
        //  public void Send_ShouldSendMessageAsync(string payload, string json);


    }
    

}
