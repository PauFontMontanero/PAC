using CsvHelper;
using FastReport;
using FastReport.Data;
using FastReport.Export.PdfSimple;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
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
        public RelayCommand ExportToPdfCommand { get; set; }

        public ClientViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            AddClientCommand = new RelayCommand(x => AddClient());
            UpdateClientCommand = new RelayCommand(x => UpdateClient());
            DelClientCommand = new RelayCommand(x => DelClient());
            ShowDataCommand = new RelayCommand(x => ShowDataClient());
            ExportToPdfCommand = new RelayCommand(x => ExportToPdf());
        }

        private void ExportToPdf()
        {
            try
            {
                // Create and load the report
                Report report = new Report();
                report.Load("Reports/ClientsList.frx");

                // Register clients data
                report.RegisterData(Clients, "Clients");

                // Enable the DataSource
                DataSourceBase dataSource = report.GetDataSource("Clients");
                dataSource.Enabled = true;

                // Link DataSource to data band
                DataBand? dataBand = report.FindObject("Data1") as DataBand;
                if (dataBand != null)
                {
                    dataBand.DataSource = dataSource;
                }

                // Prepare the report
                report.Prepare();

                // Export to PDF using MemoryStream
                using (MemoryStream ms = new MemoryStream())
                {
                    PDFSimpleExport pdfExport = new PDFSimpleExport();
                    report.Export(pdfExport, ms);

                    // Save PDF to Documents folder
                    string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    string pdfPath = Path.Combine(documentsPath, "ClientsReport.pdf");
                    System.IO.File.WriteAllBytes(pdfPath, ms.ToArray());

                    // Open the generated PDF
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = pdfPath,
                        UseShellExecute = true
                    });

                    MessageBox.Show("Report generated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
