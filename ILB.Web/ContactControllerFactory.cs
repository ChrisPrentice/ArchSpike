using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;

namespace ILB.Web
{
    public class ContactControllerFactory : DefaultControllerFactory
    {
        private readonly IContainer container;
        private readonly Dictionary<IController, ILifetimeScope> scopes;
        private readonly object syncRoot;

        public ContactControllerFactory(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.container = container;
            this.scopes = new Dictionary<IController, ILifetimeScope>();
            this.syncRoot = new object();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            var scope = this.container.BeginLifetimeScope();
            var controller = (IController)scope.Resolve(controllerType);
            lock (this.syncRoot)
            {
                this.scopes.Add(controller, scope);
            }
            return controller;
        }

        public override void ReleaseController(IController controller)
        {
            lock (this.syncRoot)
            {
                var scope = this.scopes[controller];
                this.scopes.Remove(controller);

                scope.Dispose();
            }
            base.ReleaseController(controller);
        }
    }
}