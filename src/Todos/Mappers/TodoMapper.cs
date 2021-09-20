using AutoMapper;
using Todos.Models;

namespace Todos.Mappers
{
    public class TodoMapper : Profile
    {
        public TodoMapper()
        {
            CreateMap<Todo, TodoDto>().ReverseMap();
        }
    }
}