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

        // Validate Name
        if (string.IsNullOrWhiteSpace(NewClient.Name))
        {
            errors.Add("Name cannot be empty.");
        }
        else if (!char.IsUpper(NewClient.Name[0]))
        {
            errors.Add("Name must start with an uppercase letter.");
        }

        // Validate Surname
        if (string.IsNullOrWhiteSpace(NewClient.Surname))
        {
            errors.Add("Surname cannot be empty.");
        }
        else if (!char.IsUpper(NewClient.Surname[0]))
        {
            errors.Add("Surname must start with an uppercase letter.");
        }

        // Validate Email
        if (string.IsNullOrWhiteSpace(NewClient.Email))
        {
            errors.Add("Email cannot be empty.");
        }
        else if (!IsValidEmail(NewClient.Email))
        {
            errors.Add("Invalid email format.");
        }

        // Validate Telephone
        if (NewClient.Telephone == null || NewClient.Telephone.ToString().Length != 9)
        {
            errors.Add("Telephone must be 9 digits.");
        }

        // Validate Created Date
        if (!NewClient.Created.HasValue)
        {
            errors.Add("Date of Registration cannot be empty.");
        }

        return errors;
    }

    private bool IsValidEmail(string email)
    {
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern);
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
