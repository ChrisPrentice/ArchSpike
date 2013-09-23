using ILB.ApplicationServices.Contacts;

namespace ILB.ApplicationServices
{
    /// <summary>
    /// The command invoker is responsible for write operation, each of these 
    /// commands should be an atomic unit, so it hanldes the unit of work.
    /// </summary>
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