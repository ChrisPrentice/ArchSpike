using Autofac;
using Autofac.Core.Activators.Reflection;
using Autofac.Features.ResolveAnything;
using ILB.ApplicationServices.Contacts;
using ILb.Infrastructure;
using ILB.Web.Controllers;

namespace ILB.Web
{
    public class ContactModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
            
            builder.RegisterAssemblyTypes(typeof (ContactController).Assembly)
                .AsSelf()
                .InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof (ContactRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(ContactService).Assembly, typeof(ValidationService).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}