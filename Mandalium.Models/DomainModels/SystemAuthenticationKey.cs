using System.ComponentModel.DataAnnotations;

namespace Mandalium.Models.DomainModels
{
    public class SystemAuthenticationKey
    {
        [Key,Required]
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
