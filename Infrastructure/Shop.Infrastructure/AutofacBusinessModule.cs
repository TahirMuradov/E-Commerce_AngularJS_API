using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;



namespace Shop.Infrastructure
{
   public class AutofacBusinessModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var coreAssembly = System.Reflection.Assembly.Load("Shop.Application");
            var infrastructureAssembly = System.Reflection.Assembly.Load("Shop.Infrastructure");
            var persistenceAssembly = System.Reflection.Assembly.Load("Shop.Persistence");

            builder.RegisterAssemblyTypes(coreAssembly, infrastructureAssembly, persistenceAssembly)
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                });
                
        }
    }
}
