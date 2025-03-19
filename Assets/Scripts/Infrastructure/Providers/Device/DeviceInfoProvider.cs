using UnityEngine;

namespace Infrastructure.Providers.Device
{
    public class DeviceInfoProvider
    {
        public string GetDeviceUniqueId() =>
            SystemInfo.deviceUniqueIdentifier;
    }
}