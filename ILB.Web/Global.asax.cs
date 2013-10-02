using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
namespace ILB.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var builder = new ContainerBuilder();
            builder.RegisterModule<ILB.Web.ContactModule>();

            var controllerFactory = new ILB.Web.ContactControllerFactory(builder.Build());

            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

        }
    }
}