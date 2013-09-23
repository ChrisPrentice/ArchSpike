using System.Web.Mvc;
using ILB.ApplicationServices;
using ILB.ApplicationServices.Contacts;
using ILB.Contacts;
using ILB.Infrastructure;

namespace ILB.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly ICommandInvoker commandInvoker;
        private readonly IQueryInvoker queryInvoker;

        public ContactController()
        {
            // You'd really DI this is from autofac.
            commandInvoker = new CommandInvoker();
            queryInvoker = new QueryInvoker();
        }

        public ActionResult Index()
        {
            return View(queryInvoker.Query<AllContactsQueryResult>());
        }

        public ActionResult Create()
        {
            return View(queryInvoker.Query<CreateContactQueryResult>());
        }

        [HttpPost]
        public ActionResult Create(CreateContactCommand command)
        {
            var response = commandInvoker.Execute<CreateContactCommand, CreateContactQueryResult>(command);
            return ModelState.IsValid ? RedirectToAction("Index") : (ActionResult)View(response);
        }

        public ActionResult Update(int id)
        {
            return View(queryInvoker.Query<UpdateContactQuery, UpdateContactQueryResult>(new UpdateContactQuery { Id = id }));
        }

        [HttpPost]
        public ActionResult Update(UpdateContactCommand command)
        {
            var response = commandInvoker.Execute<UpdateContactCommand, UpdateContactQueryResult>(command);
            return ModelState.IsValid ? RedirectToAction("Index") : (ActionResult)View(response);
        }
    }
}
