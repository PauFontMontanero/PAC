using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using WPF_MVVM_SPA_Template.Models;
using WPF_MVVM_SPA_Template.ViewModels;
using WPF_MVVM_SPA_Template.Views;

internal class AddClientViewModel : INotifyPropertyChanged
{
    private readonly MainViewModel _mainViewModel;
    private readonly ClientViewModel _clientViewModel;

    public RelayCommand AcceptChangesCommand { get; set; }
    public RelayCommand DeclineCommand { get; set; }

    private Client _newClient;

    public Client NewClient
    {
        get { return _newClient; }
        set { _newClient = value; OnPropertyChanged(); }
    }

    public AddClientViewModel(MainViewModel mainViewModel, ClientViewModel clientViewModel)
    {
        _newClient = new Client { Id = clientViewModel.Clients.Count + 1 };
        _mainViewModel = mainViewModel;
        _clientViewModel = clientViewModel;
        AcceptChangesCommand = new RelayCommand(x => AcceptChanges());
        DeclineCommand = new RelayCommand(x => DeclineChanges());
    }

    private void AcceptChanges()
    {
        var errors = ValidateFields();

        if (errors.Count > 0)
        {
            MessageBox.Show(string.Join("\n", errors), "Validation Errors", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        else
        {
            _clientViewModel.Clients.Add(NewClient);
            _mainViewModel.CurrentView = new ClientView { DataContext = _mainViewModel.ClientVM };
        }
    }

    private List<string> ValidateFields()
    {
        var errors = new List<string>();

        // Validate Created Date
        if (!NewClient.Created.HasValue)
        {
            errors.Add("Date of Registration cannot be empty.");
        }

        return errors;
    }

    private void DeclineChanges()
    {
        _mainViewModel.CurrentView = new ClientView { DataContext = _mainViewModel.ClientVM };
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
