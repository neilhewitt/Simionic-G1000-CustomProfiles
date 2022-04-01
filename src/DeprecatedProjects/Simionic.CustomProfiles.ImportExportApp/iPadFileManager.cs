using iMobileDevice;
using iMobileDevice.Afc;
using iMobileDevice.HouseArrest;
using iMobileDevice.iDevice;
using iMobileDevice.Lockdown;
using iMobileDevice.Plist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Simionic.CustomProfiles.ImportExportApp
{
    public  class iPadFileManager
    {
        private List<(string Name, string UDID)> _devices = new List<(string Name, string UDID)>();
        private const string APP_ID = "Simionic.CustomProfile.DesktopApp";

        public IEnumerable<string> Devices => _devices.Select(x => x.Name);

        public byte[] ExtractAppSharedFileFromiPad(string deviceName, string fileName)
        {
            if (PushPull(deviceName, fileName, false, null, out byte[] dataRead))
            {
                return dataRead;
            }
            else
            {
                throw new AfcException("Could not read the file from the device.");
            }
        }

        public void PushAppSharedFileToiPad(string deviceName, string fileName, byte[] data)
        {
            if (PushPull(deviceName, fileName, true, data, out byte[] dataRead))
            {
                return;
            }
            else
            {
                throw new AfcException("Could not push the file to the device.");
            }
        }

        private bool PushPull(string deviceName, string fileName, bool write, byte[] dataToWrite, out byte[] dataRead)
        {
            bool success = false;
            dataRead = null;

            try
            {

                var selectedDevice = _devices.Where(x => x.Name.ToLower() == deviceName.ToLower()).FirstOrDefault();
                if (selectedDevice == default((string, string))) throw new Exception("Device unavailable.");

                IiDeviceApi deviceApi = LibiMobileDevice.Instance.iDevice;
                ILockdownApi lockdownApi = LibiMobileDevice.Instance.Lockdown;
                IAfcApi afcApi = LibiMobileDevice.Instance.Afc;
                IHouseArrestApi houseArrestApi = LibiMobileDevice.Instance.HouseArrest;
                IPlistApi plistApi = LibiMobileDevice.Instance.Plist;

                deviceApi.idevice_new(out iDeviceHandle device, selectedDevice.UDID).ThrowOnError();

                lockdownApi.lockdownd_client_new_with_handshake(device, out LockdownClientHandle lockdown, APP_ID).ThrowOnError();
                lockdownApi.lockdownd_start_service(lockdown, "com.apple.afc", out LockdownServiceDescriptorHandle lockdownDescriptor).ThrowOnError();

                houseArrestApi.house_arrest_client_start_service(device, out HouseArrestClientHandle houseArrest, APP_ID).ThrowOnError();
                houseArrestApi.house_arrest_send_command(houseArrest, "VendDocuments", "com.koalar.CCHW").ThrowOnError();
                houseArrestApi.afc_client_new_from_house_arrest_client(houseArrest, out AfcClientHandle afc).ThrowOnError();

                fileName = $"Documents/{fileName}";

                ulong fileHandle = 0;

                if (write)
                {
                    uint bytesWritten = 0;
                    afcApi.afc_file_open(afc, fileName, AfcFileMode.FopenWr, ref fileHandle).ThrowOnError();
                    afcApi.afc_file_write(afc, fileHandle, dataToWrite, (uint)dataToWrite.Length, ref bytesWritten).ThrowOnError();
                    afcApi.afc_file_close(afc, fileHandle);
                    afc.Close();
                    if (bytesWritten == (uint)dataToWrite.Length) success = true;

                    dataRead = null;
                }
                else
                {
                    afcApi.afc_get_file_info(afc, fileName, out ReadOnlyCollection<string> fileInfo).ThrowOnError();
                    uint size = uint.Parse(fileInfo[1]);

                    uint bytesRead = 0;
                    byte[] buffer = new byte[size];
                    afcApi.afc_file_open(afc, fileName, AfcFileMode.FopenRdonly, ref fileHandle).ThrowOnError();
                    afcApi.afc_file_read(afc, fileHandle, buffer, size, ref bytesRead).ThrowOnError();
                    afcApi.afc_file_close(afc, fileHandle);
                    afc.Close();
                    if (bytesRead == size) success = true;

                    dataRead = buffer;
                }

                houseArrest.Close();
                lockdownDescriptor.Close();
                lockdown.Close();
                device.Close();

                afc.Dispose();
                houseArrest.Dispose();
                lockdownDescriptor.Dispose();
                lockdown.Dispose();
                device.Dispose();
            }
            catch
            {
                // swallow it... which is bad, I know, but honestly I don't need better error handling right now
            }

            return success;
        }

        public void GetDevices()
        {
            IiDeviceApi deviceApi = LibiMobileDevice.Instance.iDevice;
            ILockdownApi lockdownApi = LibiMobileDevice.Instance.Lockdown;

            int count = 0;
            iDeviceError error = deviceApi.idevice_get_device_list(out ReadOnlyCollection<string> deviceIds, ref count);

            if (error == iDeviceError.NoDevice) return; // nothing connected, this is fine
            error.ThrowOnError(); // any other error throws an exception here

            _devices.Clear();
            foreach (string deviceId in deviceIds)
            {
                deviceApi.idevice_new(out iDeviceHandle deviceHandle, deviceId).ThrowOnError();
                lockdownApi.lockdownd_client_new_with_handshake(deviceHandle, out LockdownClientHandle lockdownHandle, "Simionic.CustomProfile.DesktopApp").ThrowOnError();
                lockdownApi.lockdownd_get_device_name(lockdownHandle, out string deviceName).ThrowOnError();

                _devices.Add((deviceName, deviceId));

                deviceHandle.Dispose();
                lockdownHandle.Dispose();
            }
        }

        public iPadFileManager()
        {
            GetDevices();
        }
    }
}
