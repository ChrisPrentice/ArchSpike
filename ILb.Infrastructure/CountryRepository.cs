using System.Collections.Generic;
using System.Linq;
using ILB.Contacts;

namespace ILB.Infrastructure
{
    public class CountryRepository : ICountryRepository
    {
        private readonly List<Country> countries = new List<Country>
                {
                    new Country(1, "United Kingdom"),
                    new Country(2, "France"),
                    new Country(3, "Ireland"),
                    new Country(4, "Germany"),
                };

        public IList<Country> GetAll()
        {
            return countries;
        }

        public Country GetById(int countyId)
        {
            return countries.FirstOrDefault(q => q.Id == countyId);
        }
    }
}