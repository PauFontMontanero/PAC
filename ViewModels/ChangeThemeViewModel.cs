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
    class ChangeThemeViewModel : INotifyPropertyChanged
    {
        // Referència al ViewModel principal
        private readonly MainViewModel _mainViewModel;
        public RelayCommand ChangeThemeCommand { get; set; }

        public ChangeThemeViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            ChangeThemeCommand = new RelayCommand(x => ChangeTheme());

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
