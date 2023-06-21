using DevScheduler.API.Entities;
using DevScheduler.API.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevScheduler.API.Controllers
{
    [Route("api/dev-schedule")]
    [ApiController]
    public class DevScheduleController : ControllerBase
    {
        private readonly DevScheduleDbContent _context;
        public DevScheduleController(DevScheduleDbContent context) {
            _context = context;
        }
        [HttpGet]
        public ActionResult GetAll() {
            var devSchedule = _context.DevSchedules.Where(d => !d.IsDeleted).ToList();
            return Ok(devSchedule);
        }

        [HttpGet("{id}")]
        public ActionResult GetById(Guid id) {
            var devSchedule = _context.DevSchedules.SingleOrDefault(d => d.Id == id);
            if(devSchedule == null) {
                return NotFound();
            }
            return Ok(devSchedule);
        }

        [HttpPost]
        public ActionResult Post(DevSchedule devSchedule) {
            _context.DevSchedules.Add(devSchedule); 
            return CreatedAtAction(nameof(GetById), new {id = devSchedule.Id}, devSchedule);
        }

        [HttpPut("{id}")]
        public ActionResult Update(Guid id, DevSchedule input) {
            var devSchedule = _context.DevSchedules.SingleOrDefault(d => d.Id == id);
            if (devSchedule == null)
            {
                return NotFound();
            }
            devSchedule.Update(input.Title, input.Description, input.StartDate, input.EndDate);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id) {
            var devSchedule = _context.DevSchedules.SingleOrDefault(d => d.Id == id);
            if (devSchedule == null)
            {
                return NotFound();
            }
            devSchedule.Delete();
            return NoContent();
        }

        [HttpPost("{id}/speakers")]
        public ActionResult PostSpeaker(Guid id, DevScheduleSpeaker speaker) {
            var devSchedule = _context.DevSchedules.SingleOrDefault(d => d.Id == id);
            if (devSchedule == null)
            {
                return NotFound();
            }
            devSchedule.Speakers.Add(speaker);
            return NoContent();
        }

    }
}
