using CommunityToolkit.Mvvm.ComponentModel;

namespace MonkeyFinder.ViewModels;

[QueryProperty("Monkey", "Monkey")]
public partial class MonkeyDetailsViewModel : BaseViewModel
{
    public MonkeyDetailsViewModel()
    {
    }

    [ObservableProperty]
    private Monkey _monkey;
}
