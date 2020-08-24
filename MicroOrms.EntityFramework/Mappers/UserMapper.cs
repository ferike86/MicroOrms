using AutoMapper;
using MicroOrms.Entities;
using System.Collections.Generic;

namespace MicroOrms.EntityFramework.Mappers
{
    public static class UserMapper
    {
        private static readonly IMapper todoItemMapper = TodoItemMapper.Mapper;

        private static readonly IConfigurationProvider mapperConfiguration = new MapperConfiguration(config =>
        {
            config.CreateMap<user, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.name))
            .ForMember(dest => dest.TodoItems, opt => opt.MapFrom(src => todoItemMapper.Map<IEnumerable<TodoItem>>(src.todo_item)));
            config.CreateMap<User, user>()
            .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.todo_item, opt => opt.MapFrom(src => todoItemMapper.Map<IEnumerable<todo_item>>(src.TodoItems)));
        });

        public static IMapper Mapper => mapperConfiguration.CreateMapper();
    }
}
