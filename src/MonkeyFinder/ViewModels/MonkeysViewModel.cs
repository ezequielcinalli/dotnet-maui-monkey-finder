using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;

namespace MonkeyFinder.ViewModels;

public partial class MonkeysViewModel : BaseViewModel
{
    private readonly IMonkeyService _monkeyService;
    public ObservableCollection<Monkey> Monkeys { get; } = new();
    private readonly IConnectivity _connectivity;
    private readonly IGeolocation _geolocation;
    public MonkeysViewModel(IMonkeyService monkeyService, IConnectivity connectivity, IGeolocation geolocation)
    {
        _monkeyService = monkeyService;
        _connectivity = connectivity;
        _geolocation = geolocation;
        Title = "Monkey Finder";
    }

    [RelayCommand] 
    private async Task GetClosestMonkey()
    {
        if (IsBusy || Monkeys.Count == 0) return;

        try
        {
            var location = await _geolocation.GetLastKnownLocationAsync();
            if (location is null)
            {
                location = await _geolocation.GetLocationAsync(
                    new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
            }

            if (location is null) return;

            var first = Monkeys.MinBy(m => location.CalculateDistance(m.Latitude, m.Longitude, DistanceUnits.Kilometers));

            await Shell.Current.DisplayAlert("Closest Monkey", $"{first.Name} in {first.Location}", "OK");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error!", $"Unable to get closest monkey: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task GoToDetailsAsync(Monkey monkey)
    {
        if (monkey is null)
        {
            return;
        }

        await Shell.Current.GoToAsync($"{nameof(DetailsPage)}",
            true,
            new Dictionary<string, object>
            {
                {"Monkey", monkey}
            });
    }

    [RelayCommand]
    private async Task GetMonkeys()
    {
        if (IsBusy) return;
        try
        {
            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("Internet issue!","Check your internet and try again", "OK");
                return;
            }

            IsBusy = true;
            var monkeys = await _monkeyService.GetMonkeys();
            if (Monkeys.Count != 0)
            {
                Monkeys.Clear();
            }

            foreach (var monkey in monkeys)
            {
                Monkeys.Add(monkey);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error!", $"Unable to get monkeys: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
