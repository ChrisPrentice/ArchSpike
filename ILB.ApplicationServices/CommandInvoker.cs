using ILB.ApplicationServices.Contacts;

namespace ILB.ApplicationServices
{
    public class CommandInvoker : ICommandInvoker
    {
        private readonly ContactService contactService;

        public CommandInvoker(ContactService contactService) // this would really be resolved by autofac
        {
            this.contactService = contactService;
        }

        public void Execute<TCommand>(TCommand command)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                ((IHandleCommand<TCommand>)contactService).Execute(command);
                unitOfWork.Complete();
            }
        }

        public TResponse Execute<TCommand, TResponse>(TCommand command)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var result = ((IHandleCommand<TCommand, TResponse>)contactService).Execute(command);
                unitOfWork.Complete();
                return result;
            }
        }
    }
}