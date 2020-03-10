using Autofac;
using TransactionManager.Components.Converters;
using TransactionManager.Components.Services.Implementation;
using TransactionManager.Components.Validators.Implementations;
using TransactionManager.DAL.Repositories.Implementations;

namespace TransactionManager.Components.Infrastructure
{
    public class TransactionManagerAutofacModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TransactionManagerService>()
                .AsImplementedInterfaces();

            builder.RegisterType<FileValidator>()
                .AsImplementedInterfaces();

            builder.RegisterType<TransactionRepository>()
                .AsImplementedInterfaces();

            builder.RegisterType<StatusTypeConverter>()
                .AsImplementedInterfaces();
        }
    }
}
