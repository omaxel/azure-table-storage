using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Todos.Extensions;
using Todos.Models;
using Todos.Services;

namespace Todos.Controllers
{
    [Route("api/todos")]
    public class TodosController : Controller
    {
        private readonly TableService<Todo> _todoService;
        private readonly IMapper _mapper;

        public TodosController(TableService<Todo> todoService, IMapper mapper)
        {
            _todoService = todoService;
            _mapper = mapper;
        }

        public async Task<PageDto<TodoDto>> GetTodosAsync(CancellationToken cancellationToken, string continuationToken = null)
        {
            var result = await _todoService.GetAllAsync(null, continuationToken, cancellationToken: cancellationToken).FirstOrDefaultAsync();

            return new PageDto<TodoDto>
            {
                ContinuationToken = result.ContinuationToken,
                Values = _mapper.Map<IReadOnlyCollection<TodoDto>>(result.Values)
            };
        }

        [HttpPost]
        public async Task<TodoDto> AddAsync(TodoDto todo, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Todo>(todo);
            await _todoService.AddAsync(entity, cancellationToken);

            return _mapper.Map<TodoDto>(entity);
        }

        [HttpPut("{partitionKey}/{rowKey}")]
        public async Task<ActionResult<TodoDto>> UpdateAsync(string partitionKey, string rowKey, TodoDto todo, CancellationToken cancellationToken)
        {
            if (partitionKey != todo.PartitionKey || rowKey != todo.RowKey)
                return BadRequest("partitionKey and rowKey must match the ones in the body.");

            var entity = _mapper.Map<Todo>(todo);
            await _todoService.UpdateAsync(entity, cancellationToken);

            return Ok(_mapper.Map<TodoDto>(entity));
        }

        [HttpDelete("{partitionKey}/{rowKey}")]
        public async Task<IActionResult> DeleteAsync(string partitionKey, string rowKey, CancellationToken cancellationToken)
        {
            var entity = await _todoService.GetAsync(partitionKey, rowKey, cancellationToken);

            if (entity == null)
                return NotFound();

            await _todoService.DeleteAsync(partitionKey, rowKey, cancellationToken);

            return Ok();
        }
    }
}
