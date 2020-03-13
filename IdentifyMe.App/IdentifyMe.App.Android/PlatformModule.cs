using Autofac;
using IdentifyMe.App.Services;

namespace IdentifyMe.App.Droid
{
    public class PlatformModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule(new CoreModule());
            builder.RegisterModule(new ServicesModule());
        }
    }
}