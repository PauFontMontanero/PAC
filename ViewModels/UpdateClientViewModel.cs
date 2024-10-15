using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using WPF_MVVM_SPA_Template.Models;
using WPF_MVVM_SPA_Template.Views;

namespace WPF_MVVM_SPA_Template.ViewModels
{
    internal class UpdateClientViewModel : INotifyPropertyChanged
    {
        private readonly MainViewModel _mainViewModel;
        private readonly ClientViewModel _clientViewModel;

        public RelayCommand AcceptChangesCommand { get; set; }
        public RelayCommand DeclineCommand { get; set; }

        private Client? _oldClient;
        private Client? _selectedClient;

        public Client? SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                if (_selectedClient != value)
                {
                    _selectedClient = value;
                    if (_selectedClient != null)
                    {
                        _oldClient = new Client(_selectedClient);
                    }
                    OnPropertyChanged();
                }
            }
        }

        public UpdateClientViewModel(MainViewModel mainViewModel, ClientViewModel clientViewModel)
        {
            _mainViewModel = mainViewModel;
            _clientViewModel = clientViewModel;
            AcceptChangesCommand = new RelayCommand(x => AcceptChanges());
            DeclineCommand = new RelayCommand(x => DeclineChanges());
        }

        private void AcceptChanges()
        {
            if (_selectedClient != null)
            {
                var errors = ValidateFields();

                if (errors.Count > 0)
                {
                    MessageBox.Show(string.Join("\n", errors), "Validation Errors", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    // Find the existing client in the Clients collection
                    var existingClient = _clientViewModel.Clients.FirstOrDefault(c => c.Id == _selectedClient.Id);
                    var index = _clientViewModel.Clients.IndexOf(existingClient);
                    if (existingClient != null)
                    {
                        // Update the properties of the existing client
                        _clientViewModel.Clients[index] = new Client(_selectedClient);
                    }
                    _mainViewModel.CurrentView = new ClientView { DataContext = _mainViewModel.ClientVM };
                }
            }
        }

        private List<string> ValidateFields()
        {
            var errors = new List<string>();

            // Name validation
            if (string.IsNullOrWhiteSpace(SelectedClient?.Name))
            {
                errors.Add("Name cannot be empty.");
            }
            else if (!char.IsUpper(SelectedClient.Name[0]))
            {
                errors.Add("Name must start with an uppercase letter.");
            }

            // Surname validation
            if (string.IsNullOrWhiteSpace(SelectedClient?.Surname))
            {
                errors.Add("Surname cannot be empty.");
            }
            else if (!char.IsUpper(SelectedClient.Surname[0]))
            {
                errors.Add("Surname must start with an uppercase letter.");
            }

            // Email validation
            if (string.IsNullOrWhiteSpace(SelectedClient?.Email))
            {
                errors.Add("Email cannot be empty.");
            }
            else if (!IsValidEmail(SelectedClient.Email))
            {
                errors.Add("Invalid email format.");
            }

            // Telephone validation
            if (SelectedClient.Telephone == null || SelectedClient.Telephone.ToString().Length != 9)
            {
                errors.Add("Telephone must be 9 digits.");
            }

            // Created Date validation
            if (!SelectedClient.Created.HasValue)
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
            if (_oldClient != null && _selectedClient != null)
            {
                _selectedClient = new Client(_oldClient);
                OnPropertyChanged(nameof(SelectedClient));
            }
            _mainViewModel.CurrentView = new ClientView { DataContext = _mainViewModel.ClientVM };
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
