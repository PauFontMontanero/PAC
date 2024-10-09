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
        private readonly ClientViewModel _option1ViewModel;
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

        public UpdateClientViewModel(MainViewModel mainViewModel, ClientViewModel option1ViewModel)
        {
            _mainViewModel = mainViewModel;
            _option1ViewModel = option1ViewModel; // Store the reference
            AcceptChangesCommand = new RelayCommand(x => AcceptChanges());
            DeclineCommand = new RelayCommand(x => DeclineChanges());
        }

        private void AcceptChanges()
        {
            if (_selectedClient != null)
            {
                // Find the existing client in the Clients collection
                var existingClient = _option1ViewModel.Clients.FirstOrDefault(c => c.Id == _selectedClient.Id);
                var index = _option1ViewModel.Clients.IndexOf(existingClient);
                if (existingClient != null)
                {
                    // Update the properties of the existing client
                    _option1ViewModel.Clients[index] = new Client(_selectedClient);
                }
            }
            _mainViewModel.CurrentView = new ClientView { DataContext = _mainViewModel.Option1VM };
        }

        private void DeclineChanges()
        {
            if (_oldClient != null && _selectedClient != null)
            {
                _selectedClient = new Client(_oldClient);

                // Revert other properties as needed
                OnPropertyChanged(nameof(SelectedClient));
            }
            _mainViewModel.CurrentView = new ClientView { DataContext = _mainViewModel.Option1VM };
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
