using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoList.Api.Models;
using TodoList.Api.ViewModels;

namespace TodoList.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class StepsController : ControllerBase
    {

        private readonly TodoListDbContext _context;
        public StepsController(TodoListDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Step>>> GetSteps()
        {
            return await _context.Steps.OrderByDescending(x => x.Id).ToListAsync();
        }

        [HttpGet("{taskId}/steps")]
        public async Task<ActionResult<IEnumerable<Step>>> GetStepsByTaskId(int taskId)
        {
            return await _context.Steps.Where(x => x.TaskId == taskId).OrderByDescending(x => x.Id).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Step>> GetStep(int id)
        {
            var step = await _context.Steps.FindAsync(id);
            if (step == null)
                return BadRequest(new { message = "Step không tồn tại"});

            return step;
        }

        [HttpPost]
        public async Task<ActionResult<Step>> PostStep(CreateStepRequest request)
        {
            var step = new Step
            {
                Title = request.Title,
                TaskId = request.TaskId,
                IsComplete = false
            };

            _context.Steps.Add(step);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStep", new { id = step.Id }, step);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStep(int id, CreateStepRequest request)
        {
            var step = await _context.Steps.FindAsync(id);
            if (step == null)
                return BadRequest(new { message = "Step không tồn tại"});
            step.Title = request.Title;
            step.TaskId = request.TaskId;
            step.IsComplete = request.IsComplete;
            
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStep", new { id = step.Id }, step);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStep(int id)
        {
            var step = await _context.Steps.FindAsync(id);
            if (step == null)
                return BadRequest(new { message = "Step không tồn tại"});
            _context.Steps.Remove(step);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}