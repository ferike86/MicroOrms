using System.Collections.Generic;
using System.Data.SqlClient;
using AutoMapper;
using Dapper.Contrib.Extensions;

namespace MicroOrms.Dapper.Contrib
{
    public class TodoDatabase : ITodoDatabase
    {
        private static readonly IConfigurationProvider mapperConfiguration;
        private static readonly IMapper mapper;
        private readonly string dbConnectionString;

        static TodoDatabase()
        {
            mapperConfiguration = new MapperConfiguration(config =>
            {
                config.CreateMap<DapperContribTodoItem, TodoItem>()
                .ForMember(dest => dest.IsComplete, opt => opt.MapFrom(src => src.Is_Complete));
                config.CreateMap<TodoItem, DapperContribTodoItem>()
                .ForMember(dest => dest.Is_Complete, opt => opt.MapFrom(src => src.IsComplete));
            });
            mapper = mapperConfiguration.CreateMapper();
        }

        public TodoDatabase(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;

        }

        public long Create(TodoItem todoItem)
        {
            using (var connection = new SqlConnection(dbConnectionString))
            {
                return connection.Insert(mapper.Map<DapperContribTodoItem>(todoItem));
            }
        }

        public bool Delete(long id)
        {
            using (var connection = new SqlConnection(dbConnectionString))
            {
                var todoItemToDelete = connection.Get<DapperContribTodoItem>(id);
                return connection.Delete(todoItemToDelete);
            }
        }

        public TodoItem Read(long id)
        {
            using (var connection = new SqlConnection(dbConnectionString))
            {
                return mapper.Map<TodoItem>(connection.Get<DapperContribTodoItem>(id));
            }
        }

        public IEnumerable<TodoItem> ReadAll()
        {
            using (var connection = new SqlConnection(dbConnectionString))
            {
                return mapper.Map<IEnumerable<TodoItem>>(connection.GetAll<DapperContribTodoItem>());
            }
        }

        public bool Update(TodoItem todoItem)
        {
            using (var connection = new SqlConnection(dbConnectionString))
            {
                return connection.Update(mapper.Map<DapperContribTodoItem>(todoItem));
            }
        }
    }
}
