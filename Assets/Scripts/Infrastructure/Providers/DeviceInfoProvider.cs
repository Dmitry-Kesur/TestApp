using UnityEngine;

namespace Infrastructure.Providers
{
    public class DeviceInfoProvider
    {
        public string GetDeviceUniqueId() =>
            SystemInfo.deviceUniqueIdentifier;
    }
}