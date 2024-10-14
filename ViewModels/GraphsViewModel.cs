using LiveCharts.Wpf;
using LiveCharts;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WPF_MVVM_SPA_Template.Models;
using System.Windows.Input;

namespace WPF_MVVM_SPA_Template.ViewModels
{
    class GraphsViewModel : INotifyPropertyChanged
    {
        private readonly MainViewModel _mainViewModel;
        private Client _client;

        public ICommand LineChartButton_Click { get; set; }
        public ICommand BarChartButton_Click { get; set; }

        private SeriesCollection _seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get => _seriesCollection;
            set
            {
                _seriesCollection = value;
                OnPropertyChanged();
            }
        }
        public string[] Months { get; } = new[]
        {
            "January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        };

        public GraphsViewModel(MainViewModel mainViewModel, Client client)
        {
            _mainViewModel = mainViewModel;
            _client = client;

            LineChartButton_Click = new RelayCommand(x => LineChart());
            BarChartButton_Click = new RelayCommand(x => BarChart());

            // Initialize with a default chart type
            LineChart();
        }

        private void LineChart()
        {
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = $"{_client.Name}'s Monthly Values",
                    Values = new ChartValues<int>(Client.RandomMonthlyValues) // Replace with actual monthly data if available
                }
            };
        }

        private void BarChart()
        {
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = $"{_client.Name}'s Monthly Values",
                    Values = new ChartValues<int>(Client.RandomMonthlyValues) // Replace with actual monthly data if available
                }
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
