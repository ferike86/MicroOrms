using Autofac;
using System.Configuration;

namespace MicroOrms.User.Autofac
{
    public class TodoDatabaseModule : Module
    {
        private static ConnectionStringSettings DbConfiguration => ConfigurationManager.ConnectionStrings["TodoDatabase"];

        private static ConnectionStringSettings EntityFrameworkDbConfiguration => ConfigurationManager.ConnectionStrings["TodoContext"];

        public OrmType OrmType { get; set; } = OrmType.Dapper;

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            ITodoDatabase todoDatabase;

            switch (OrmType)
            {
                case OrmType.Dapper:
                    todoDatabase = new Dapper.TodoDatabase(DbConfiguration.ConnectionString);
                    break;
                case OrmType.DapperContrib:
                    todoDatabase = new Dapper.Contrib.TodoDatabase(DbConfiguration.ConnectionString);
                    break;
                case OrmType.AdoNet:
                    todoDatabase = new AdoNet.TodoDatabase(DbConfiguration.ConnectionString);
                    break;
                case OrmType.LinqToDb:
                    todoDatabase = new LinqToDb.TodoDatabase(DbConfiguration.Name);
                    break;
                case OrmType.SqlFu:
                    todoDatabase = new SqlFu.TodoDatabase(DbConfiguration.ConnectionString);
                    break;
                case OrmType.EntityFramework:
                    todoDatabase = new EntityFramework.TodoDatabase(EntityFrameworkDbConfiguration.ConnectionString);
                    break;
                default:
                    todoDatabase = new Dapper.TodoDatabase(DbConfiguration.ConnectionString);
                    break;
            }
            builder.RegisterInstance(todoDatabase).As<ITodoDatabase>();
        }
    }
}
