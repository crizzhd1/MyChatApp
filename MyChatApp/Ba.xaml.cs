
namespace MyChatApp;

public partial class Ba : ContentPage
{
	bool isToggled = false;

    public Ba()
	{
		InitializeComponent();
	}

    async void ToggelLight(object sender, EventArgs e)
    {
        isToggled = !isToggled;

        try
        {
            if (isToggled)
            {

                await Flashlight.Default.TurnOnAsync();

            }
            else
            {
                await Flashlight.Default.TurnOffAsync();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    public void ToggleAccelerometer(object sender, EventArgs e)
    {
        if (Accelerometer.Default.IsSupported)
        {
            if (!Accelerometer.Default.IsMonitoring)
            {
                // Turn on accelerometer
                Accelerometer.Default.ReadingChanged += Accelerometer_ReadingChanged;
                Accelerometer.Default.Start(SensorSpeed.UI);
            }
            else
            {
                // Turn off accelerometer
                Accelerometer.Default.Stop();
                Accelerometer.Default.ReadingChanged -= Accelerometer_ReadingChanged;
            }
        }
    }

    private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
    {
        // Update UI Label with accelerometer state
        WAlk.TextColor = Colors.Green;
        WAlk.Text = $"Accel: {e.Reading}";
    }
}
