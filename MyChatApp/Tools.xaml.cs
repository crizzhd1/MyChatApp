using CommunityToolkit.Maui.Storage;
using Plugin.Maui.Audio;

namespace MyChatApp;

public partial class Tools : ContentPage
{
	bool isToggled = false;
    readonly IAudioManager _audioManager;
    readonly IAudioRecorder _audioRecorder;

    public Tools(IAudioManager audioManager)
    {
		InitializeComponent();

        _audioManager = audioManager;
        _audioRecorder = audioManager.CreateRecorder();
    }

    public async void ToggePhonelLight(object sender, EventArgs e)
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
        catch (FeatureNotSupportedException)
        {
            await DisplayAlert("Not supported", "Flashlight is not supported on this device.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    public async void RecordAudio(object sender, EventArgs e)
    {
        try
        {
            if (await Permissions.RequestAsync<Permissions.Microphone>() != PermissionStatus.Granted) return;

            if (!_audioRecorder.IsRecording)
            {
                await _audioRecorder.StartAsync();
                RecStat.Text = "Recording... Tap to stop.";
            }
            else
            {
                var audioStream = await _audioRecorder.StopAsync();
                await SaveAudioToUserLocationAsync(audioStream.GetAudioStream());
                RecStat.Text = "Stoped Recording... and Saved";
            }
        }

        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
            return;
        }
    }

    async Task SaveAudioToUserLocationAsync(Stream audioStream)
    {
        var fileName = $"recording_{DateTime.Now:yyyyMMdd_HHmmss}.wav";

        using var memoryStream = new MemoryStream();
        await audioStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        var platform = DeviceInfo.Current.Platform;

        if (platform == DevicePlatform.WinUI || platform == DevicePlatform.MacCatalyst)
        {
            // ✅ Desktop: use FileSaver
            try
            {
                var result = await FileSaver.Default.SaveAsync(fileName, memoryStream);
                if (result.IsSuccessful)
                    await Shell.Current.DisplayAlert("Saved", $"File saved to {result.FilePath}", "OK");
                else
                    await Shell.Current.DisplayAlert("Error", result.Exception?.Message ?? "Save failed", "OK");
            }
            catch (NotImplementedException)
            {
                await Shell.Current.DisplayAlert("Error", "File saver not supported on this platform.", "OK");
            }
        }
        else
        {
            // ✅ Mobile fallback
            try
            {
                var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
                using var fileStream = File.Create(filePath);
                memoryStream.Position = 0;
                await memoryStream.CopyToAsync(fileStream);
                await Shell.Current.DisplayAlert("Saved", $"Saved to: {filePath}", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}

