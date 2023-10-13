using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundJobs.Client.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly IBackgroundJobClient BackgroundJobClient;

        private readonly ILogger<JobsController> _logger;

        public JobsController(ILogger<JobsController> logger, IBackgroundJobClient backgroundJobClient)
        {
            _logger = logger;
            BackgroundJobClient = backgroundJobClient;
        }


        [HttpGet("CreateEnqueue")]
        public ActionResult Enqueue()
        {
            BackgroundJobClient.Enqueue(() => Console.WriteLine("Tarea enqueue ejecutada"));
            return Ok();
        }

        [HttpGet("CreateSchedule")]
        public ActionResult Schedule()
        {
            BackgroundJobClient.Schedule(() => Console.WriteLine("Tarea schedule ejecutada"), TimeSpan.FromSeconds(5));
            return Ok();
        }

        [HttpGet("CreateRecurring")]
        public ActionResult Recurring()
        {
            RecurringJob.AddOrUpdate("Tarea 1", () => Console.WriteLine("Tarea reucurring ejecutada"), Cron.Minutely);
            return Ok();
        }

        [HttpGet("CreateRecurring2")]
        public ActionResult Recurring2()
        {
            RecurringJob.AddOrUpdate("Tarea 2", () => Console.WriteLine("Tarea reucurring ejecutada"), Cron.Minutely);
            return Ok();
        }

    }
}

