using ILB.ApplicationServices;
using ILB.ApplicationServices.Contacts;
using ILB.Contacts;
using ILb.Infrastructure;

namespace ILB.Infrastructure
{
    public class QueryInvoker : IQueryInvoker
    {

        private static IHandleQuery<TQuery, TQueryResult> ResolveContactService<TQuery, TQueryResult>()
        {
            return (IHandleQuery<TQuery, TQueryResult>)CreateContactService();
        }

        private static IHandleQuery<TQueryResult> ResolveContactService<TQueryResult>()
        {
            return (IHandleQuery<TQueryResult>)CreateContactService();
        }

        /// <summary>
        /// THis would come from Autofac
        /// </summary>
        /// <returns></returns>
        private static ContactService CreateContactService()
        {
            return new ContactService(new CountyRepository(),
                                      new CountryRepository(),
                                      new ContactRepository(),
                                      new ValidationService(),
                                      new ContactAdministrationService(new CountyRepository(),
                                                                       new CountryRepository(),
                                                                       new ContactRepository()));
        }

        public TQueryResult Query<TQueryResult>()
        {
            return ResolveContactService<TQueryResult>().Query();
        }

        public TQueryResult Query<TQuery, TQueryResult>(TQuery query)
        {
            return ResolveContactService<TQuery, TQueryResult>().Query(query);
        }
    }
}