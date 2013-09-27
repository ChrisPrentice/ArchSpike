using System.ComponentModel.DataAnnotations;

namespace ILB.Contacts
{
    public class CreateContactCommand
    {
        [Required]
        public string FirstName { get; set; }
        //TODO don't allow special characters
        [Required]
        public string Surname { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        //Todo Allow positive int values
        [Range(2, int.MaxValue)]
        public int? CountyId { get; set; }

        [Required]
        public int? CountryId { get;  set; }
    }
}