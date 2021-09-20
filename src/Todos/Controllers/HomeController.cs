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
    [Route("todos")]
    public class HomeController : Controller
    {
        private readonly TableService<Todo> _todoService;
        private readonly IMapper _mapper;

        public HomeController(TableService<Todo> todoService, IMapper mapper)
        {
            _todoService = todoService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var todos = await _todoService.GetAllAsync(cancellationToken: cancellationToken).FirstOrDefaultAsync();

            ViewData["Todos"] = _mapper.Map<IEnumerable<TodoDto>>(todos.Values);
            ViewData["TodosContinuationToken"] = todos.ContinuationToken;

            return View();
        }

        [HttpGet("add")]
        public IActionResult Add() => View();

        [HttpGet("edit/{partitionKey}/{rowKey}")]
        public async Task<IActionResult> Edit(string partitionKey, string rowKey, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(partitionKey) || string.IsNullOrEmpty(rowKey))
            {
                ViewData["Todo"] = $"{nameof(partitionKey)} or {nameof(rowKey)} can't be null or empty.";
                return View();
            }

            var todo = await _todoService.GetAsync(partitionKey, rowKey, cancellationToken);

            if (todo == null)
                ViewData["Error"] = "No todo found with the specified partitionKey and rowKey.";
            else
                ViewData["Todo"] = _mapper.Map<TodoDto>(todo);

            return View();
        }
    }
}