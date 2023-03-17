using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoList.Api.ViewModels;
using TodoList.Api.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using TodoList.Api.Services;
using Microsoft.Extensions.Configuration;

namespace TodoList.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class TasksController : ControllerBase
    {
        private readonly TodoListDbContext _context;
        private readonly IStorageService _storageService;
        private readonly IConfiguration _config;
        public TasksController(TodoListDbContext context, IStorageService storageService, IConfiguration config)
        {
            _context = context;
            _storageService = storageService;
            _config = config;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Task>> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            task.Steps = await _context.Steps.Where(x => x.TaskId == id).ToListAsync();
            task.Files = await _context.Files.Where(x => x.TaskId == id).ToListAsync();
            if (task == null)
                return BadRequest(new { message = "Task không tồn tại" });

            return task;
        }

        [HttpPost]
        public async Task<ActionResult> PostTask(CreateTaskRequest request)
        {
            var task = new Models.Task
            {
                Title = request.Title,
                CreatedDate = DateTime.Now,
                IsComplete = false,
                IsImportant = request.IsImportant,
                CreatedBy = request.CreatedBy
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTask", new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, CreateTaskRequest request)
        {
            var task = await _context.Tasks.FindAsync(id);
            task.Steps = await _context.Steps.Where(x => x.TaskId == id).ToListAsync();
            task.Files = await _context.Files.Where(x => x.TaskId == id).ToListAsync();
            if (task == null)
                return BadRequest(new { message = "Task không tồn tại" });
            task.Title = request.Title;
            task.DueDate = request.DueDate;
            task.IsComplete = request.IsComplete;
            task.IsImportant = request.IsImportant;
            task.CreatedBy = request.CreatedBy;

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTask", new { id = task.Id }, task);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            var files = await _context.Files.Where(x => x.TaskId == id).ToListAsync();
            if (task == null)
                return BadRequest(new { message = "Task không tồn tại" });
            _context.Tasks.Remove(task);

            foreach (var file in files)
            {
                await _storageService.DeleteFileAsync(file.Path.Split('/')[2]);
            }
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{userId}/tasks")]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetTasks(Guid userId)
        {
            return await _context.Tasks
            .Where(x => x.CreatedBy == userId)
            .OrderByDescending(x => x.Id)
            .Select(
                x => new Models.Task
                {
                    Id = x.Id,
                    Title = x.Title,
                    DueDate = x.DueDate,
                    IsComplete = x.IsComplete,
                    IsImportant = x.IsImportant,
                    CreatedBy = x.CreatedBy,
                    Steps = _context.Steps.Where(s => s.TaskId == x.Id).ToList(),
                    Files = _context.Files.Where(s => s.TaskId == x.Id).ToList()
                }

            ).ToListAsync();
        }

        [HttpGet("{userId}/tasks/{searchString}")]
        public async Task<ActionResult<IEnumerable<Models.Task>>> SearchTask(Guid userId, string searchString)
        {
            var tasks = await _context.Tasks
            .Where(x => x.CreatedBy == userId && x.Title.ToLower().Contains(searchString.ToLower()))
            .OrderByDescending(x => x.Id)
            .Select(
                x => new Models.Task
                {
                    Id = x.Id,
                    Title = x.Title,
                    DueDate = x.DueDate,
                    IsComplete = x.IsComplete,
                    IsImportant = x.IsImportant,
                    CreatedBy = x.CreatedBy,
                    Steps = _context.Steps.Where(s => s.TaskId == x.Id).ToList()
                }

            ).ToListAsync();
            if (tasks == null)
                return BadRequest(new { message = "Danh sách trống" });

            return tasks;
        }

        [HttpGet("{userId}/tasks/sortby/{predicate}")]
        public async Task<ActionResult<IEnumerable<Models.Task>>> SortByPredicate(Guid userId, string predicate)
        {
            switch (predicate)
            {
                case "importance":
                    return await _context.Tasks
                    .Where(x => x.CreatedBy == userId)
                    .OrderByDescending(x => x.IsImportant)
                    .Select(
                        x => new Models.Task
                        {
                            Id = x.Id,
                            Title = x.Title,
                            DueDate = x.DueDate,
                            IsComplete = x.IsComplete,
                            IsImportant = x.IsImportant,
                            CreatedBy = x.CreatedBy,
                            Steps = _context.Steps.Where(s => s.TaskId == x.Id).ToList()
                        }

                    ).ToListAsync();

                case "dueDate":
                    return await _context.Tasks
                    .Where(x => x.CreatedBy == userId)
                    .OrderBy(x => DateTime.Today)
                    .ThenBy(x => x.DueDate)
                    .Select(
                        x => new Models.Task
                        {
                            Id = x.Id,
                            Title = x.Title,
                            DueDate = x.DueDate,
                            IsComplete = x.IsComplete,
                            IsImportant = x.IsImportant,
                            CreatedBy = x.CreatedBy,
                            Steps = _context.Steps.Where(s => s.TaskId == x.Id).ToList()
                        }

                    ).ToListAsync();

                default:
                    return null;
            }
        }

        [HttpPost("{taskId}/files")]
        public async Task<IActionResult> AddFile(int taskId, IFormFile file)
        {
            var fileModel = new Models.File()
            {
                Name = file.FileName,
                Size = file.Length,
                Type = file.FileName.Split('.')[1],
                Path = await this.SaveFile(file),
                TaskId = taskId,
            };
            _context.Files.Add(fileModel);
            await _context.SaveChangesAsync();

            var task = await _context.Tasks.FindAsync(taskId);
            task.Steps = await _context.Steps.Where(x => x.TaskId == taskId).ToListAsync();
            task.Files = await _context.Files.Where(x => x.TaskId == taskId).ToListAsync();
            return CreatedAtAction("GetTask", new { id = taskId }, task);
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{file.FileName.Split('.')[0]}-{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return _storageService.GetFileUrl(fileName);
        }
    }
}