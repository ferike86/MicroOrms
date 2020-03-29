using Autofac;
using System.Configuration;

namespace MicroOrms.User.Autofac
{
    public class TodoDatabaseModule : Module
    {
        private static string DbConnectionString => ConfigurationManager.ConnectionStrings["TodoDatabase"].ConnectionString;

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            var todoDatabase = new Dapper.Contrib.TodoDatabase(DbConnectionString);
            builder.RegisterInstance(todoDatabase).As<ITodoDatabase>();
        }
    }
}
