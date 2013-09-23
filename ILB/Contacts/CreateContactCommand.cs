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
        public int CountyId { get; set; }
        public int CountryId { get;  set; }
    }
}