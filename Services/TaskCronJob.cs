using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TodoList.Api.Services
{
    public class TaskCronJob : CronJobService
    {
        private readonly ILogger<TaskCronJob> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        public TaskCronJob(IScheduleConfig<TaskCronJob> config, ILogger<TaskCronJob> logger, IServiceScopeFactory scopeFactory)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Task starts.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<TodoListDbContext>();

                System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("vi-VN");
                var task = _context.Tasks
                .Where(x => x.DueDate >= DateTime.Now)
                .OrderBy(x => x.DueDate)
                .FirstOrDefault();

                // var cronItem = Convert.ToDateTime(task.DueDate).ToString("yyyy/MM/dd HH:mm:ss").Split('/', ':', ' ');
                // string dayOfWeek = Convert.ToDateTime(task.DueDate).DayOfWeek.ToString("d");
                // string cronEx = cronItem[5] + " " + cronItem[4] + " " + cronItem[3] + " " + cronItem[2] + " " + cronItem[1] + " " + dayOfWeek;
                Console.WriteLine("Task: " + Convert.ToDateTime(task.DueDate).ToString("MM/dd/yyyy HH:mm"));
                Console.WriteLine("Now: " + DateTime.Now.ToString("MM/dd/yyyy HH:mm"));
                
            }
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Task is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}