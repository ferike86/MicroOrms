using AutoMapper;
using MicroOrms.Entities;

namespace MicroOrms.EntityFramework.Mappers
{
    public static class TodoItemMapper
    {
        private static readonly IConfigurationProvider mapperConfiguration = new MapperConfiguration(config =>
        {
            config.CreateMap<todo_item, TodoItem>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
            .ForMember(dest => dest.IsComplete, opt => opt.MapFrom(src => src.is_complete))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.name))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.user_id));
            config.CreateMap<TodoItem, todo_item>()
            .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.is_complete, opt => opt.MapFrom(src => src.IsComplete))
            .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.user_id, opt => opt.MapFrom(src => src.UserId));
        });

        public static IMapper Mapper => mapperConfiguration.CreateMapper();
    }
}
