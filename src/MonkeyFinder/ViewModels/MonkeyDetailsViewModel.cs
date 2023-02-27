using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MonkeyFinder.ViewModels;

[QueryProperty("Monkey", "Monkey")]
public partial class MonkeyDetailsViewModel : BaseViewModel
{
    private IMap _map;
    public MonkeyDetailsViewModel(IMap map)
    {
        _map = map;
    }

    [ObservableProperty]
    private Monkey _monkey;

    [RelayCommand]
    private async Task OpenMapAsync()
    {
        try
        {
            await _map.OpenAsync(Monkey.Latitude, Monkey.Longitude, new MapLaunchOptions
            {
                Name = Monkey.Name,
                NavigationMode = NavigationMode.None
            });
        }
        catch(Exception ex)
        {
           Debug.WriteLine(ex);
           await Shell.Current.DisplayAlert("Error!", $"Unable to open map: {ex.Message}", "OK");
        }
    }
}
