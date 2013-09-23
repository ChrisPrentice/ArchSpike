using System.ComponentModel.DataAnnotations;

namespace ILB.Contacts
{
    public class CreateContactCommand
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Surname { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        
        [Range(2, int.MaxValue)]
        public int? CountyId { get; set; }

        [Required]
        public int? CountryId { get;  set; }
    }
}