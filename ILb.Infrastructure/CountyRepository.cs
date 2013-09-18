using System.Collections.Generic;
using System.Linq;
using ILB.Contacts;

namespace ILB.Infrastructure
{
    public class CountyRepository : ICountyRepository
    {
        private readonly List<County> counties = new List<County>
                {
                    new County(1, "Aberdeenshire"),
                    new County(2, "Anglesey"),
                    new County(3, "Angus"),
                    new County(4, "Argyllshire"),
                    new County(5, "Ayrshire"),
                    new County(6, "Banffshire")
                };

        public IList<County> GetAll()
        {
            return counties;
        }

        public County GetById(int countyId)
        {
            return counties.FirstOrDefault(q => q.Id == countyId);
        }
    }
}