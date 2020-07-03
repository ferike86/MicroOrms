using AutoMapper;
using MicroOrms.Entities;
using MicroOrms.LinqToDb.Entities;
using System.Collections.Generic;

namespace MicroOrms.LinqToDb.Mappers
{
    public static class UserMapper
    {
        private static readonly IMapper todoItemMapper = TodoItemMapper.Mapper;

        private static readonly IConfigurationProvider mapperConfiguration = new MapperConfiguration(config =>
        {
            config.CreateMap<LinqToDbUser, User>()
            .ForMember(dest => dest.TodoItems, opt => opt.MapFrom(src => todoItemMapper.Map<IEnumerable<TodoItem>>(src.TodoItems)));
            config.CreateMap<User, LinqToDbUser>()
            .ForMember(dest => dest.TodoItems, opt => opt.MapFrom(src => todoItemMapper.Map<IEnumerable<LinqToDbUser>>(src.TodoItems)));
        });

        public static IMapper Mapper => mapperConfiguration.CreateMapper();
    }
}
