using Autofac;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MicroOrms.User.Autofac
{
    public class TodoDatabaseModule : Module
    {
        private static ConnectionStringSettings DbConfiguration => ConfigurationManager.ConnectionStrings["TodoDatabase"];

        private static ConnectionStringSettings EntityFrameworkDbConfiguration => ConfigurationManager.ConnectionStrings["TodoContext"];

        private static readonly Func<IDbConnection> ConnectionFactory = () => new SqlConnection(DbConfiguration.ConnectionString);

        public OrmType OrmType { get; set; } = OrmType.Dapper;

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            ITodoDatabase todoDatabase;

            switch (OrmType)
            {
                case OrmType.Dapper:
                    todoDatabase = new Dapper.TodoDatabase(ConnectionFactory);
                    break;
                case OrmType.DapperContrib:
                    todoDatabase = new Dapper.Contrib.TodoDatabase(ConnectionFactory);
                    break;
                case OrmType.AdoNet:
                    todoDatabase = new AdoNet.TodoDatabase(ConnectionFactory);
                    break;
                case OrmType.LinqToDb:
                    todoDatabase = new LinqToDb.TodoDatabase(DbConfiguration.Name);
                    break;
                case OrmType.SqlFu:
                    todoDatabase = new SqlFu.TodoDatabase(SqlFu.ProviderType.SqlServer2012, DbConfiguration.ConnectionString);
                    break;
                case OrmType.EntityFramework:
                    todoDatabase = new EntityFramework.TodoDatabase(EntityFrameworkDbConfiguration.ConnectionString);
                    break;
                default:
                    todoDatabase = new Dapper.TodoDatabase(ConnectionFactory);
                    break;
            }
            builder.RegisterInstance(todoDatabase).As<ITodoDatabase>();
        }
    }
}
