using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Minitab_Assignment
{
    public class Address
    {
        public Address(string line1, string city, string state, string postalCode, string country)
        {
            Line1 = line1;
            City = city;
            State = state;
            PostalCode = postalCode;
            Country = country;
        }

        [Required]
        public string Line1 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Country { get; set; }

        public override string ToString() => string.Join(", ", from p in GetType().GetProperties() select $"\"{p.Name}\"=\"{p.GetValue(this)}\"");
    }
}
