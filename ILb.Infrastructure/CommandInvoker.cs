using ILB.ApplicationServices;
using ILB.ApplicationServices.Contacts;
using ILB.Contacts;
using ILb.Infrastructure;

namespace ILB.Infrastructure
{
    public class CommandInvoker : ICommandInvoker
    {

        private static IHandleCommand<TCommand> ResolveContactService<TCommand>()
        {
            return (IHandleCommand<TCommand>)CreateContactService();
        }

        private static IHandleCommand<TCommand, TCommandResult> ResolveContactService<TCommand, TCommandResult>()
        {
            return (IHandleCommand<TCommand, TCommandResult>)CreateContactService();
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

        public void Execute<T>(T command)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                ResolveContactService<T>().Execute(command);
                unitOfWork.Complete();
            }
        }

        public TResponse Execute<TCommand, TResponse>(TCommand command)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var result =  ResolveContactService<TCommand, TResponse>().Execute(command);
                unitOfWork.Complete();
                return result;
            }
        }
    }
}