using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WPF_MVVM_SPA_Template.Models
{
    public class Client : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private string _surname;
        private string _email;
        private int? _telephone;
        private DateTime? _created;
        private int[] _randomMonthlyValues;

        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                if (_surname != value)
                {
                    _surname = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }

        public int? Telephone
        {
            get => _telephone;
            set
            {
                if (_telephone != value)
                {
                    _telephone = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime? Created
        {
            get => _created;
            set
            {
                if (_created != value)
                {
                    _created = value;
                    OnPropertyChanged();
                }
            }
        }

        public int[] RandomMonthlyValues
        {
            get => _randomMonthlyValues;
            set
            {
                if (_randomMonthlyValues != value)
                {
                    _randomMonthlyValues = value;
                    OnPropertyChanged();
                }
            }
        }

        public Client()
        {
            RandomMonthlyValues = GenerateRandomValues();
        }

        public Client(Client existingClient)
        {
            if (existingClient == null)
            {
                throw new ArgumentNullException(nameof(existingClient), "The existing client cannot be null.");
            }

            Id = existingClient.Id;
            Name = existingClient.Name;
            Surname = existingClient.Surname;
            Email = existingClient.Email;
            Telephone = existingClient.Telephone;
            Created = existingClient.Created;
            RandomMonthlyValues = existingClient.RandomMonthlyValues;
        }

        private static int[] GenerateRandomValues()
        {
            Random random = new Random();
            return Enumerable.Range(1, 12).Select(_ => random.Next(1, 100)).ToArray();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
