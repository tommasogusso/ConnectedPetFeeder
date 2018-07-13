using Microsoft.Azure.Devices.Client;
using System;
using System.Configuration;

namespace ManualConnectedCiotola
{
    class Program
    {
        static void Main(string[] args)
        {
            var deviceID = "manualconnectedcciotola" /*ConfigurationManager.AppSettings["deviceId"]*/;
            var authenticationMethod = new DeviceAuthenticationWithRegistrySymmetricKey(
                deviceID, "XiJLJz5eb/fmhNixjpAG6SN6I7/fl0wg+jSV0TFjqi8=" /*ConfigurationManager.AppSettings["deviceKey"]*/);
            var transportType = TransportType.Mqtt;
            //if (!string.IsNullOrWhiteSpace(/*ConfigurationManager.AppSettings["transportType"]*/))
            //{
            //    transportType = (TransportType)Enum.Parse(typeof(TransportType),
            //        ConfigurationManager.AppSettings["transportType"]);
            //}

            var client = DeviceClient.Create(
                ""
                /*ConfigurationManager.AppSettings["hostName"]*/,
                authenticationMethod,
                transportType
            );

            var ciotolaMoel = new CiotolaModel(deviceID, client);
        }
    }
}
