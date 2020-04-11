using Autofac;
using System.Configuration;

namespace MicroOrms.User.Autofac
{
    public class TodoDatabaseModule : Module
    {
        private static string DbConnectionString => ConfigurationManager.ConnectionStrings["TodoDatabase"].ConnectionString;

        public OrmType OrmType { get; set; } = OrmType.Dapper;

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            ITodoDatabase todoDatabase;

            switch (OrmType)
            {
                case OrmType.Dapper:
                    todoDatabase = new Dapper.TodoDatabase(DbConnectionString);
                    break;
                case OrmType.DapperContrib:
                    todoDatabase = new Dapper.Contrib.TodoDatabase(DbConnectionString);
                    break;
                default:
                    todoDatabase = new Dapper.TodoDatabase(DbConnectionString);
                    break;
            }
            builder.RegisterInstance(todoDatabase).As<ITodoDatabase>();
        }
    }
}
