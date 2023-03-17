using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoList.Api.Models;
using TodoList.Api.Services;

namespace TodoList.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class FilesController : ControllerBase
    {
        private readonly TodoListDbContext _context;
        private readonly IStorageService _storageService;
        public FilesController(TodoListDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.File>>> GetFiles()
        {
            return await _context.Files.OrderByDescending(x => x.Id).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Models.File>> GetFile(int id)
        {
            var file = await _context.Files.FindAsync(id);
            if (file == null)
                return BadRequest(new { message = "File không tồn tại" });

            return file;
        }

        [HttpGet("{taskId}/files")]
        public async Task<ActionResult<IEnumerable<File>>> GetFilesByTaskId(int taskId)
        {
            return await _context.Files.Where(x => x.TaskId == taskId).OrderByDescending(x => x.Id).ToListAsync();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            var file = await _context.Files.FindAsync(id);
            if (file == null)
                return BadRequest(new { message = "File không tồn tại" });
            await _storageService.DeleteFileAsync(file.Path.Split('/')[2]);
            _context.Files.Remove(file);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}