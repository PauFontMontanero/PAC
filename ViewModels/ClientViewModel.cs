using CsvHelper;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Xml.Linq;
using WPF_MVVM_SPA_Template.Models;
using WPF_MVVM_SPA_Template.Views;

namespace WPF_MVVM_SPA_Template.ViewModels
{
    class ClientViewModel : INotifyPropertyChanged
    {
        private readonly MainViewModel _mainViewModel;

        public ObservableCollection<Client> Clients { get; set; } = new ObservableCollection<Client>();

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

        public RelayCommand AddClientCommand { get; set; }
        public RelayCommand UpdateClientCommand { get; set; }
        public RelayCommand DelClientCommand { get; set; }
        public RelayCommand ShowDataCommand { get; set; }

        public ClientViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            AddClientCommand = new RelayCommand(x => AddClient());
            UpdateClientCommand = new RelayCommand(x => UpdateClient());
            DelClientCommand = new RelayCommand(x => DelClient());
            ShowDataCommand = new RelayCommand(x => ShowDataClient());
        }

        public void ImportClients(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Client>().ToList();
                Clients.Clear();
                foreach (var record in records)
                {
                    Clients.Add(record);
                }
            }
        }

        public void ExportClients(string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(Clients);
            }
        }

        private void AddClient()
        {
            AddClientViewModel AddClientVM = new AddClientViewModel(_mainViewModel, this);
            _mainViewModel.CurrentView = new AddClientView { DataContext = AddClientVM };
        }

        private void DelClient()
        {
            if (MessageBox.Show("Vols borrar el client?", "Confirm delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (SelectedClient != null)
                    Clients.Remove(SelectedClient);
            }
        }

        private void UpdateClient()
        {
            if (SelectedClient != null)
            {
                UpdateClientViewModel updateClientVM = new UpdateClientViewModel(_mainViewModel, this)
                {
                    SelectedClient = new Client(SelectedClient)
                };
                _mainViewModel.CurrentView = new UpdateClientView { DataContext = updateClientVM };
            }
        }

        private void ShowDataClient()
        {
            if (SelectedClient != null)
            {
                GraphsViewModel graphsVM = new GraphsViewModel(_mainViewModel, SelectedClient);
                _mainViewModel.CurrentView = new GraphsView { DataContext = graphsVM };
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
