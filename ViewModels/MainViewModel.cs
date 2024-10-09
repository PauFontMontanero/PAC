using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using WPF_MVVM_SPA_Template.Views;

namespace WPF_MVVM_SPA_Template.ViewModels
{
    //Els ViewModels deriven de INotifyPropertyChanged per poder fer Binding de propietats
    class MainViewModel : INotifyPropertyChanged
    {

        // ViewModels de les diferents opcions
        public ClientViewModel Option1VM { get; set; }
        public UpdateClientViewModel UpdateClientVM { get; set; }
        public AddClientViewModel AddClientVM { get; set; }

        public RelayCommand ChangeThemeCommand { get; set; }

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
                _selectedView = value;
                OnPropertyChanged();
                ChangeView();
            }
        }

        public MainViewModel()
        {
            // Inicialitzem els diferents ViewModels
            Option1VM = new ClientViewModel(this);
            
            // Mostra la vista principal inicialment
            SelectedView = "Option1";
            ChangeView();
            ChangeThemeCommand = new RelayCommand(x => ChangeTheme());

        }

        // Canvi de Vista
        private void ChangeView()
        {
            switch (SelectedView)
            {
                case "Option1": CurrentView = new ClientView { DataContext = Option1VM }; break;
            }
        }

        private bool _isDarkTheme;
        private void ChangeTheme()
        {
            ResourceDictionary theme = new ResourceDictionary();

            if (_isDarkTheme)
            {
                theme.Source = new Uri("pack://application:,,,/Views/Themes/ModernTheme.xaml");
            }
            else
            {
                theme.Source = new Uri("pack://application:,,,/Views/Themes/ModernDarkTheme.xaml");
            }

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(theme);

            _isDarkTheme = !_isDarkTheme;
        }


        // Això és essencial per fer funcionar el Binding de propietats entre Vistes i ViewModels
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
