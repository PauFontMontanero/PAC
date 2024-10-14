using System;
using System.Linq;

namespace WPF_MVVM_SPA_Template.Models
{
    class Client
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public int? Telephone { get; set; }
        public DateTime? Created { get; set; }

        public static int[] RandomMonthlyValues { get; } = GenerateRandomValues();

        public Client() { }

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
        }

        private static int[] GenerateRandomValues()
        {
            Random random = new Random();
            return Enumerable.Range(1, 12).Select(_ => random.Next(1, 100)).ToArray();
        }
    }
}
