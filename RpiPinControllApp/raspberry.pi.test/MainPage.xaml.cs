using raspberry.pi.test.Controls;
using raspberry.pi.test.models;
using System;
using System.Collections.Generic;
using Windows.Devices.Gpio;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace raspberry.pi.test
{
    public sealed partial class MainPage : Page
    {
        private readonly GpioController _gpioController;

        public MainPage()
        {
            InitializeComponent();
            _gpioController = GpioController.GetDefault();
        }

        private void PinControl_OpenPin(object sender, OpenPinEventArgs e)
        {
            e.Pin = _gpioController.OpenPin(e.PinId);
        }
    }
}