using AutoMapper;
using MicroOrms.Dapper.Contrib.Entities;
using MicroOrms.Entities;
using System.Collections.Generic;

namespace MicroOrms.Dapper.Contrib.Mappers
{
    public static class UserMapper
    {
        private static readonly IMapper todoItemMapper = TodoItemMapper.Mapper;

        private static readonly IConfigurationProvider mapperConfiguration = new MapperConfiguration(config =>
        {
            config.CreateMap<DapperContribUser, User>()
            .ForMember(dest => dest.TodoItems, opt => opt.MapFrom(src => todoItemMapper.Map<IEnumerable<TodoItem>>(src.TodoItems)));
            config.CreateMap<User, DapperContribUser>()
            .ForMember(dest => dest.TodoItems, opt => opt.MapFrom(src => todoItemMapper.Map<IEnumerable<DapperContribTodoItem>>(src.TodoItems)));
        });

        public static IMapper Mapper => mapperConfiguration.CreateMapper();
    }
}
