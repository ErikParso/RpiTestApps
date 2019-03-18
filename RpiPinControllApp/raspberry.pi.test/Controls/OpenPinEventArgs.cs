using System;
using Windows.Devices.Gpio;

namespace raspberry.pi.test.Controls
{
    public class OpenPinEventArgs : EventArgs
    {
        public OpenPinEventArgs(int pinId)
        {
            PinId = pinId;
        }

        public GpioPin Pin { get; set; }
        public int PinId { get; }
    }
}
