using raspberry.pi.test.ViewModels;
using System;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace raspberry.pi.test.Controls
{
    public sealed partial class PinControl : UserControl
    {
        public static readonly DependencyProperty PinTypeProperty = DependencyProperty.Register(
            nameof(PinType), typeof(PinType), typeof(PinControl), new PropertyMetadata(PinType.Pin));

        public static readonly DependencyProperty PinIdProperty = DependencyProperty.Register(
            nameof(PinId), typeof(int), typeof(PinControl), new PropertyMetadata(0));

        private GpioPin _pin;

        public PinControl()
        {
            InitializeComponent();
            Actualize();
        }

        public PinType PinType
        {
            get { return (PinType)GetValue(PinTypeProperty); }
            set { SetValue(PinTypeProperty, value); InitializeColor(value); }
        }

        public int PinId
        {
            get { return (int)GetValue(PinIdProperty); }
            set { SetValue(PinIdProperty, value); }
        }

        public string PinName
        {
            get => PinType == PinType.Pin ? PinId.ToString() : PinType.ToString();
        }

        public event EventHandler<OpenPinEventArgs> OpenPin;

        private void Ellipse_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (PinType == PinType.Pin)
            {
                if (_pin == null)
                {
                    var openPinEventArgs = new OpenPinEventArgs(PinId);
                    OpenPin?.Invoke(this, openPinEventArgs);
                    _pin = openPinEventArgs.Pin;
                    _pin.SetDriveMode(GpioPinDriveMode.Output);
                    _pin.ValueChanged += (pin, args) => Actualize();
                }
                var newValue = _pin.Read() == GpioPinValue.High ? GpioPinValue.Low : GpioPinValue.High;
                _pin.Write(newValue);
            }
        }

        private void Actualize()
        {
            if (_pin == null)
            {
                SetEllypseColor(Colors.Transparent);
            }
            else
            {
                var pinValue = _pin.Read();
                if (pinValue == GpioPinValue.High)
                {
                    SetEllypseColor(Colors.Blue);
                }
                else
                {
                    SetEllypseColor(Colors.Gray);
                }
            }
        }

        private void InitializeColor(PinType pinType)
        {
            switch (pinType)
            {
                case PinType.Gnd: SetEllypseColor(Colors.Black); break;
                case PinType.Pow_3V: SetEllypseColor(Colors.DarkOrange); break;
                case PinType.Pow_5V: SetEllypseColor(Colors.Red); break;
                case PinType.Reserved: SetEllypseColor(Colors.DarkGray); break;
                case PinType.Uarto: SetEllypseColor(Colors.Green); break;
                case PinType.Pin: SetEllypseColor(Colors.Black); break;
            }
        }

        private async void SetEllypseColor(Color color)
            => await ellipse.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
                ellipse.Fill = new SolidColorBrush(color));
    }
}
