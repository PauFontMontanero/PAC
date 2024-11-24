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

        // Properties to track control validation states
        private bool _isNameValid;
        public bool IsNameValid
        {
            get { return _isNameValid; }
            set { _isNameValid = value; OnPropertyChanged(); }
        }

        private bool _isSurnameValid;
        public bool IsSurnameValid
        {
            get { return _isSurnameValid; }
            set { _isSurnameValid = value; OnPropertyChanged(); }
        }

        private bool _isEmailValid;
        public bool IsEmailValid
        {
            get { return _isEmailValid; }
            set { _isEmailValid = value; OnPropertyChanged(); }
        }

        private bool _isPhoneValid;
        public bool IsPhoneValid
        {
            get { return _isPhoneValid; }
            set { _isPhoneValid = value; OnPropertyChanged(); }
        }

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
                        UpdateValidationProperties();
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
                    var existingClient = _clientViewModel.Clients.FirstOrDefault(c => c.Id == _selectedClient.Id);
                    var index = _clientViewModel.Clients.IndexOf(existingClient);
                    if (existingClient != null)
                    {
                        _clientViewModel.Clients[index] = new Client(_selectedClient);
                    }
                    _mainViewModel.CurrentView = new ClientView { DataContext = _mainViewModel.ClientVM };
                }
            }
        }

        private List<string> ValidateFields()
        {
            var errors = new List<string>();

            if (!IsNameValid)
                errors.Add("Name must be at least 3 characters long.");

            if (!IsSurnameValid)
                errors.Add("Surname must be at least 3 characters long.");

            if (!IsEmailValid)
                errors.Add("Please enter a valid email address.");

            if (!IsPhoneValid)
                errors.Add("Please enter a valid phone number.");

            if (!SelectedClient?.Created.HasValue ?? true)
                errors.Add("Date of Registration cannot be empty.");

            return errors;
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

        private void UpdateValidationProperties()
        {
            IsNameValid = !string.IsNullOrWhiteSpace(_selectedClient.Name) && _selectedClient.Name.Length >= 3;
            IsSurnameValid = !string.IsNullOrWhiteSpace(_selectedClient.Surname) && _selectedClient.Surname.Length >= 3;
            IsEmailValid = IsValidEmail(_selectedClient.Email);
            IsPhoneValid = IsValidPhone(_selectedClient.Telephone);
        }

        private bool IsValidEmail(string email)
        {
            // Add your email validation logic here
            return !string.IsNullOrWhiteSpace(email);
        }

        private bool IsValidPhone(int? phone)
        {
            // Add your phone number validation logic here
            return phone.HasValue && !string.IsNullOrWhiteSpace(phone.Value.ToString());
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}