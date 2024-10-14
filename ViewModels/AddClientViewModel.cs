using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using WPF_MVVM_SPA_Template.Models;
using WPF_MVVM_SPA_Template.ViewModels;
using WPF_MVVM_SPA_Template.Views;

internal class AddClientViewModel : INotifyPropertyChanged
{
    private readonly MainViewModel _mainViewModel;

    private readonly ClientViewModel _ClientViewModel;

    // Relay commands for buttons

    public RelayCommand AcceptChangesCommand { get; set; }
    public RelayCommand DeclineCommand { get; set; }

    private Client? _newClient;
    public Client? NewClient
    {
        get { return _newClient; }
        set { _newClient = value; OnPropertyChanged(); }
    }
    public AddClientViewModel(MainViewModel mainViewModel, ClientViewModel clientViewModel)
    {
        _newClient = new Client();
        _newClient.Id = clientViewModel.Clients.Count + 1;
        _mainViewModel = mainViewModel;
        _ClientViewModel = clientViewModel;
        AcceptChangesCommand = new RelayCommand(x => AcceptChanges());
        DeclineCommand = new RelayCommand(x => DeclineChanges());
    }

    private void AcceptChanges()
    {
        // Create a new client object with the data from the TextBoxes      
        _mainViewModel.CurrentView = new ClientView { DataContext = _mainViewModel.ClientVM };
        _ClientViewModel.Clients.Add(NewClient);
    }

    private void DeclineChanges()
    {
        _mainViewModel.CurrentView = new ClientView { DataContext = _mainViewModel.ClientVM };
    }


    // Event for property change notifications
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
