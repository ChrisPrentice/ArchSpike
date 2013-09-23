using ILB.ApplicationServices.Contacts;

namespace ILB.ApplicationServices
{
    public class QueryInvoker : IQueryInvoker
    {
        private readonly ContactService contactService;

        public QueryInvoker(ContactService contactService) // this would really be resolved by autofac
        {
            this.contactService = contactService;
        }

        public TQueryResult Query<TQueryResult>()
        {
            return ((IHandleQuery<TQueryResult>)contactService).Query();
        }

        public TQueryResult Query<TQuery, TQueryResult>(TQuery query)
        {
            return ((IHandleQuery<TQuery, TQueryResult>)contactService).Query(query);
        }
    }
}