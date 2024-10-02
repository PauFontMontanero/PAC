using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WPF_MVVM_SPA_Template.Models;
using WPF_MVVM_SPA_Template.Views;

namespace WPF_MVVM_SPA_Template.ViewModels
{
    internal class UpdateClientViewModel : INotifyPropertyChanged
    {
        // Referència al ViewModel principal
        private readonly MainViewModel _mainViewModel;
        public RelayCommand AcceptChangesCommand { get; set; }
        public RelayCommand DeclineCommand { get; set; }


        // Propietat per controlar l'estudiant seleccionat a la vista

        private Client? _oldClient;

        public Client? OldClient
        {
            get { return _selectedClient; }
            set
            {
                _selectedClient = value;
                if (_selectedClient != null)
                {
                    _oldClient = _selectedClient;
                }
                OnPropertyChanged();
            }
        }

        private Client? _selectedClient;
        public Client? SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                _selectedClient = value;
                if (_selectedClient != null)
                {
                    _oldClient = _selectedClient;
                }
                OnPropertyChanged();
            }
        }

        public UpdateClientViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            AcceptChangesCommand = new RelayCommand(x => AcceptChanges());
            DeclineCommand = new RelayCommand(x => DeclineChanges());
        }


        private void AcceptChanges()
        {
            _mainViewModel.CurrentView = new Option1View { DataContext = _mainViewModel.Option1VM };
        }

        private void DeclineChanges()
        {
            if (_oldClient != null)
            {
                SelectedClient = _oldClient;
            }
            _mainViewModel.CurrentView = new Option1View { DataContext = _mainViewModel.Option1VM };
        }


        // Això és essencial per fer funcionar el Binding de propietats entre Vistes i ViewModels
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        //
    }
}
