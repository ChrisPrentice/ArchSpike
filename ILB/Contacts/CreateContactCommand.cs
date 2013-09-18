using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ILB.Contacts
{
    public class CreateContactCommand
    {
        public CreateContactCommand()
        {
            
        }

        public CreateContactCommand(IList<County> counties, IList<Country> countries)
        {
            Counties = counties;
            Countries = countries;
        }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Surname { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int CountyId { get; set; }
        public int CountryId { get;  set; }
        public IList<County> Counties { get; set; }
        public IList<Country> Countries { get; set; }
    }
}