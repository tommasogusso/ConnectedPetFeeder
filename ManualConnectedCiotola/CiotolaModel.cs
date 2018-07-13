using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ManualConnectedCiotola
{
    class CiotolaModel : INotifyPropertyChanged
    {
        private decimal _setPoint = 3;
        private DeviceClient _client;

        

        public CiotolaModel(string deviceId, DeviceClient client)
        {
            Task.Run(async () =>
            {
                var twin = await client.GetTwinAsync();
                if (twin.Properties.Desired.Contains("SetPoint"))
                {
                    _setPoint = (int)twin.Properties.Desired["SetPoint"];
                }
                if (twin.Properties.Desired.Contains("AlarmPoint"))
                {
                   
                }

                Dosi = _setPoint;

                await client.SetDesiredPropertyUpdateCallbackAsync(async (tc, oc) =>
                {
                    if (tc.Contains("SetPoint"))
                    {
                        _setPoint = (int)tc["SetPoint"];
                        Dosi = _setPoint;
                    }
                   
                }, null);
                var random = new Random();
                while (true)
                {
                    //Dosi--;

                    var sample = new
                    {
                        Timestamp = DateTime.Now.ToUniversalTime(),
                        DeviceId = deviceId,
                        SampleType = "dosi",
                        Value = Dosi
                    };
                    var json = JsonConvert.SerializeObject(sample);
                    var bytes = Encoding.UTF8.GetBytes(json);

                    var message = new Message(bytes);
                    message.Properties["sampleType"] = "dosi";
                    client.SendEventAsync(message).Wait();
                    
                    Task.Delay(1000).Wait();

                }
            });

            _client = client;
        }

        private decimal _dosi;

        public decimal Dosi
        {
            get => _dosi;
            set
            {
                _dosi = value;
               
            }
        }

        protected void Notify([CallerMemberName]string propertyName = null)
        {
            if (_propertyChanged == null) return;
            _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private PropertyChangedEventHandler _propertyChanged;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                _propertyChanged += value;
            }

            remove
            {
                _propertyChanged -= value;
            }
        }
    }
}
