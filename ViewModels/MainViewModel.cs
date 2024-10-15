
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using WPF_MVVM_SPA_Template.Views;

namespace WPF_MVVM_SPA_Template.ViewModels
{
    //Els ViewModels deriven de INotifyPropertyChanged per poder fer Binding de propietats
    class MainViewModel : INotifyPropertyChanged
    {

        // ViewModels de les diferents opcions
        public ClientViewModel ClientVM { get; set; }
        public StartViewModel StartVM { get; set; }
        public AboutUsViewModel AboutUsVM { get; set; }
        public ChangeThemeViewModel ChangeThemeVM { get; set; }
        public UpdateClientViewModel UpdateClientVM { get; set; }
        public AddClientViewModel AddClientVM { get; set; }

        public string CSVPATH = "C:\\Users\\10049183\\Source\\Repos\\PAC\\Data.csv";



        // Propietat que conté la vista actual (és un objecte)
        private object? _currentView;
        public object? CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        // Propietat per controlar la vista seleccionada al ListBox (És un string)
        private string? _selectedView;
        public string? SelectedView
        {
            get { return _selectedView; }
            set
            {
                if (_selectedView == value) return;

                _selectedView = value;
                OnPropertyChanged();
                ChangeView();
            }
        }

        public MainViewModel()
        {
            // Inicialitzem els diferents ViewModels
            ClientVM = new ClientViewModel(this);
            StartVM = new StartViewModel(this);
            ChangeThemeVM = new ChangeThemeViewModel(this);
            AboutUsVM = new AboutUsViewModel(this);
            OnApplicationStart();
            // Mostra la vista principal inicialment

            Application.Current.Exit += OnApplicationExit;
            SelectedView = "MainPage";
            ChangeView();


        }

        // Canvi de Vista
        private void ChangeView()
        {
            switch (SelectedView)
            {
                case "Client": CurrentView = new ClientView { DataContext = ClientVM }; break;
                case "MainPage": CurrentView = new StartView { DataContext = StartVM }; break;
                case "ChangeTheme": CurrentView = new ChangeThemeView {DataContext = ChangeThemeVM}; break;
                case "AboutUs": CurrentView = new AboutUsView { DataContext = AboutUsVM }; break;
            }
        }
        private void OnApplicationStart()
        {
            // Import client data when the application is opening
            ClientVM.ImportClients(CSVPATH);
        }
        private void OnApplicationExit(object sender, ExitEventArgs e)
        {
            // Export client data when the application is exiting
            ClientVM.ExportClients(CSVPATH);
        }

        // Això és essencial per fer funcionar el Binding de propietats entre Vistes i ViewModels
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
