
using Plugin.Maui.Audio;

namespace MyChatApp;

public partial class Ba : ContentPage
{
	bool isToggled = false;

    readonly IAudioManager _audioManager;
    readonly IAudioRecorder _audioRecorder;

    public Ba(IAudioManager audioManager)
	{
		InitializeComponent();

        _audioManager = audioManager;
        _audioRecorder = audioManager.CreateRecorder();
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

    async void RecordAudio(object sender, EventArgs e)
    {
        try
        {
            if (await Permissions.RequestAsync<Permissions.Microphone>() != PermissionStatus.Granted) return;

            if (!_audioRecorder.IsRecording)
            {
               await _audioRecorder.StartAsync();
            }
            else
            {
               var recordedAudio =  await _audioRecorder.StopAsync();
               var player = _audioManager.CreatePlayer(recordedAudio.GetAudioStream());
               player.Play();
            }
        }

        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
            return;
        }
    }
}
