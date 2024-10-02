using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using WPF_MVVM_SPA_Template.Models;
using WPF_MVVM_SPA_Template.Views;

namespace WPF_MVVM_SPA_Template.ViewModels
{
    //Els ViewModels deriven de INotifyPropertyChanged per poder fer Binding de propietats
    class Option1ViewModel : INotifyPropertyChanged
    {
        // Referència al ViewModel principal
        private readonly MainViewModel _mainViewModel;

        // Col·lecció de Students (podrien carregar-se d'una base de dades)
        // ObservableCollection és una llista que notifica els canvis a la vista
        public ObservableCollection<Client> Clients { get; set; } = new ObservableCollection<Client>();

        // Propietat per controlar l'estudiant seleccionat a la vista
        private Client? _oldClient;
        public Client? OldClient
        {
            get { return _oldClient; }
            set
            {
                _oldClient = value;
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
            set { _selectedClient = value; OnPropertyChanged(); }
        }

        // RelayCommands connectats via Binding als botons de la vista
        public RelayCommand AddClientCommand { get; set; }
        public RelayCommand UpdateClientCommand { get; set; }
        public RelayCommand DelClientCommand { get; set; }

        public Option1ViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            // Carreguem estudiants a memòria mode de prova
            Clients.Add(new Client { Id = 1, Name = "David", Surname = "Font", Email = "davidfont@gmail.com", Created= DateTime.Now, Telephone = 938586485});
            Clients.Add(new Client { Id = 2, Name = "Jordi", Surname = "Aumatell", Email = "jordiaumatell@gmail.com", Created = DateTime.Now, Telephone = 938586484 });

            // Inicialitzem els diferents commands disponibles (accions)
            AddClientCommand = new RelayCommand(x => AddClient());
            UpdateClientCommand = new RelayCommand(x => UpdateClient());
            DelClientCommand = new RelayCommand(x => DelClient());
        }

        //Mètodes per afegir i eliminar estudiants de la col·lecció
        private void AddClient()
        {
            AddClientViewModel AddClientVM = new AddClientViewModel(_mainViewModel, this);
            _mainViewModel.CurrentView = new AddClientView { DataContext = AddClientVM };
            //Clients.Add(new Client { Id = Clients.Count + 1, Name = "Nou" });
        }

        private void DelClient()
        {
            if (SelectedClient != null)
                Clients.Remove(SelectedClient);
        }
        private void UpdateClient()
        {
            if (SelectedClient != null)
            {
                _oldClient = SelectedClient;
                UpdateClientViewModel UpdateClientVM = new UpdateClientViewModel(_mainViewModel);
                _mainViewModel.CurrentView = new UpdateClientView { DataContext = _mainViewModel.UpdateClientVM };
            }
        }

        // Això és essencial per fer funcionar el Binding de propietats entre Vistes i ViewModels
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
