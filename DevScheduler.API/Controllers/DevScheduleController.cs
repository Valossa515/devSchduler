using DevScheduler.API.Entities;
using DevScheduler.API.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var devSchedule = _context.DevSchedules
                 .Include(de => de.Speakers)
                 .SingleOrDefault(d => d.Id == id);
            if(devSchedule == null) {
                return NotFound();
            }
            return Ok(devSchedule);
        }

        [HttpPost]
        public ActionResult Post(DevSchedule devSchedule) {
            _context.DevSchedules.Add(devSchedule);
            _context.SaveChanges();
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
            _context.DevSchedules.Update(devSchedule);
            _context.SaveChanges();
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
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPost("{id}/speakers")]
        public ActionResult PostSpeaker(Guid id, DevScheduleSpeaker speaker) {
            speaker.DevScheduleId = id;
            var devSchedule = _context.DevSchedules.Any(d => d.Id == id);
            if (!devSchedule)
            {
                return NotFound();
            }
            _context.devScheduleSpeakers.Add(speaker);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
