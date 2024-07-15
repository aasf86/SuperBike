using Microsoft.AspNetCore.Mvc;
using SuperBike.Consumer.DataAccess;
using SuperBike.Consumer.Entities;

namespace SuperBike.Consumer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceivedEventsController : ControllerBase
    {
        private readonly ILogger<ReceivedEventsController> _logger;
        private readonly DataAccessEvent _dataAccessEvent;

        public ReceivedEventsController(ILogger<ReceivedEventsController> logger, DataAccessEvent dataAccessEvent)
        {
            _logger = logger;
            _dataAccessEvent = dataAccessEvent;
        }
        
        [HttpGet("Events")]
        public async Task<IActionResult> Events()
        {
            var list = await _dataAccessEvent.GetAll<MotorcycleInsertedEvent>();

            return Ok(list);
        }
        
        [HttpGet("Count")]
        public async Task<IActionResult> Count()
        {
            return Ok((await _dataAccessEvent.GetAll<MotorcycleInsertedEvent>()).Count());
        }
    }
}
