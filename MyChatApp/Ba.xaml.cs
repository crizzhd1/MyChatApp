
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
}