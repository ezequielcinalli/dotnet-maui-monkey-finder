using CommunityToolkit.Mvvm.ComponentModel;

namespace MonkeyFinder.ViewModels;

public partial class BaseViewModel : ObservableObject
{

    public BaseViewModel()
    {
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy;

    [ObservableProperty]
    private string _title;

    public bool IsNotBusy => !IsBusy;

}
