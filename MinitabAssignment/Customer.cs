using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;

namespace Minitab_Assignment
{
    public class Customer
    {
        public Customer(string name, string email)
        {
            Name = name;
            Email = email;
        }

        [Required]
        [JsonPropertyName("CustomerName")]
        public string Name { get; set; }

        [Required]
        [JsonPropertyName("CustomerEmail")]
        public string Email { get; set; }

        public Address? Address { get; set; }

        public override string ToString() => string.Join(", ", from p in GetType().GetProperties() select $"\"{p.Name}\"=\"{p.GetValue(this)}\"");
    }
}
