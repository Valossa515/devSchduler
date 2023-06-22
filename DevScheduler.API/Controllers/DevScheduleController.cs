using Azure;
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

        /// <summary>
        /// Obter todos os eventos
        /// </summary>
        /// <returns>Coleção de eventos</returns>
        /// <response code="200">Sucesso</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult GetAll() {
            var devSchedule = _context.DevSchedules.Where(d => !d.IsDeleted).ToList();
            return Ok(devSchedule);
        }
        /// <summary>
        /// Obter um evento
        /// </summary>
        /// <param name="id">Identificador do evento</param>
        /// <returns>Dados do evento</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Não encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetById(Guid id) {
            var devSchedule = _context.DevSchedules
                 .Include(de => de.Speakers)
                 .SingleOrDefault(d => d.Id == id);
            if(devSchedule == null) {
                return NotFound();
            }
            return Ok(devSchedule);
        }
        /// <summary>
        /// Cadastrar um evento
        /// </summary>
        /// <remarks>
        /// {"title":"string","description":"string","startDate":"2023-02-27T17:59:14.141Z","endDate":"2023-02-27T17:59:14.141Z"}
        /// </remarks>
        /// <param name="devSchedule">Dados do evento</param>
        /// <returns>Objeto recém-criado</returns>
        /// <response code="201">Sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult Post(DevSchedule devSchedule) {
            _context.DevSchedules.Add(devSchedule);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new {id = devSchedule.Id}, devSchedule);
        }

        /// <summary>
        /// Atualizar um evento
        /// </summary>
        /// <remarks>
        /// {"title":"string","description":"string","startDate":"2023-02-27T17:59:14.141Z","endDate":"2023-02-27T17:59:14.141Z"}
        /// </remarks>
        /// <param name="id">Identificador do evento</param>
        /// <param name="input">Dados do evento</param>
        /// <returns>Nada.</returns>
        /// <response code="404">Não encontrado.</response>
        /// <response code="204">Sucesso</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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
        /// <summary>
        /// Deletar um evento
        /// </summary>
        /// <param name="id">Identificador de evento</param>
        /// <returns>Nada</returns>
        /// <response code="404">Não encontrado</response>
        /// <response code="204">Sucesso</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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
        /// <summary>
        /// Cadastrar palestrante
        /// </summary>
        /// <remarks>
        /// {"name":"string","talkTitle":"string","talkDescription":"string","linkedInProfile":"string"}
        /// </remarks>
        /// <param name="id">Identificador do evento</param>
        /// <param name="speaker">Dados do palestrante</param>
        /// <returns>Nada</returns>
        /// <response code="204">Sucesso</response>
        /// <response code="404">Evento não encontrado</response>
        [HttpPost("{id}/speakers")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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
