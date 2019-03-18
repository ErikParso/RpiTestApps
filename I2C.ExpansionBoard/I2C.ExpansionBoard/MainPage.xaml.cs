using AdafruitClassLibrary;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace I2C.ExpansionBoard
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Mcp23017 expander;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Interrupt(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            if(expander != null && args.Edge == GpioPinEdge.FallingEdge && expander.digitalRead(7) == Mcp23017.Level.LOW)
            {
                var inputs = expander.readGPIOAB();
                MoveLed();
            }
        }

        private async Task Init()
        {
            expander = new Mcp23017();
            await expander.InitMCP23017Async();
            expander.EnableInterrupts(7);
            expander.EnableInterruptsMirroring();
            expander.pinMode(7, Mcp23017.Direction.INPUT);
            expander.pinMode(8, Mcp23017.Direction.OUTPUT);
            expander.pinMode(9, Mcp23017.Direction.OUTPUT);
            expander.pinMode(10, Mcp23017.Direction.OUTPUT);
            MoveLed();
        }

        private int _currentPin = 0;

        private void MoveLed()
        {
            expander.writeGPIOAB(0);
            expander.digitalWrite(_currentPin + 8, Mcp23017.Level.HIGH);
            _currentPin = (_currentPin + 1) % 3;        
        }

        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await Init();
        }
    }
}