using AutoMapper;
using MicroOrms.Dapper.Contrib.Entities;
using MicroOrms.Entities;

namespace MicroOrms.Dapper.Contrib.Mappers
{
    public static class TodoItemMapper
    {
        private static readonly IConfigurationProvider mapperConfiguration = new MapperConfiguration(config =>
        {
            config.CreateMap<DapperContribTodoItem, TodoItem>()
            .ForMember(dest => dest.IsComplete, opt => opt.MapFrom(src => src.Is_Complete))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User_Id));
            config.CreateMap<TodoItem, DapperContribTodoItem>()
            .ForMember(dest => dest.Is_Complete, opt => opt.MapFrom(src => src.IsComplete))
            .ForMember(dest => dest.User_Id, opt => opt.MapFrom(src => src.UserId));
        });

        public static IMapper Mapper => mapperConfiguration.CreateMapper();
    }
}
