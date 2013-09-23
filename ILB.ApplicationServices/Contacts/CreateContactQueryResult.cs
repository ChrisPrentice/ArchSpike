using System.Collections.Generic;
using ILB.Contacts;

namespace ILB.ApplicationServices.Contacts
{
    public class CreateContactQueryResult
    {
        public CreateContactQueryResult(IList<County> counties, IList<Country> countries)
        {
            Counties = counties;
            Countries = countries;
            Command = new CreateContactCommand();
        }

        public CreateContactCommand Command { get; set; }
        public IList<County> Counties { get; set; }
        public IList<Country> Countries { get; set; }
    }
}