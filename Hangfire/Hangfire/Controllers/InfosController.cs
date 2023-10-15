using Hangfire.DataContext;
using Microsoft.AspNetCore.Mvc;

namespace Hangfire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfosController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;

        public InfosController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        [HttpPost]
        [Route("ChangeStatus")]
        public IActionResult ChangeStatus()
        {

            BackgroundJob.Enqueue<InfosController>(x=>x.UpdateStatusDaily());
            return Ok();
        }
        [HttpGet]
        public void UpdateStatusDaily()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var infos = _context.Infos.ToList();

                foreach (var info in infos)
                {
                    info.Status = false;
                }
                _context.SaveChanges();

            }
        }
    }
}
