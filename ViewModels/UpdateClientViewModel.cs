using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
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
            _clientViewModel = clientViewModel; // Store the reference
            AcceptChangesCommand = new RelayCommand(x => AcceptChanges());
            DeclineCommand = new RelayCommand(x => DeclineChanges());
        }

        private void AcceptChanges()
        {
            if (_selectedClient != null)
            {
                // Find the existing client in the Clients collection
                var existingClient = _clientViewModel.Clients.FirstOrDefault(c => c.Id == _selectedClient.Id);
                var index = _clientViewModel.Clients.IndexOf(existingClient);
                if (existingClient != null)
                {
                    // Update the properties of the existing client
                    _clientViewModel.Clients[index] = new Client(_selectedClient);
                }
            }
            _mainViewModel.CurrentView = new ClientView { DataContext = _mainViewModel.ClientVM };
        }

        private void DeclineChanges()
        {
            if (_oldClient != null && _selectedClient != null)
            {
                _selectedClient = new Client(_oldClient);

                // Revert other properties as needed
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
