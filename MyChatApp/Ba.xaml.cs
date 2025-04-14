
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
                await Flashlight.Default.TurnOffAsync();

            }
            else
            {
                await Flashlight.Default.TurnOnAsync();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}