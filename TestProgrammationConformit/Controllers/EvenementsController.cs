using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProgrammationConformit.Entities;
using TestProgrammationConformit.Infrastructures;

namespace TestProgrammationConformit.Controllers
{
    public class EvenementsController : BaseApiController
    {
        private readonly ConformitContext _context;
        public EvenementsController(ConformitContext context)
        {
            _context = context;
        }

        //return all Events in pages
        [HttpGet("{page}/{pagesize}")]
        public async Task<ActionResult<IEnumerable<Evenement>>> GetEvents(int page = 1, int pagesize = 10)
        {
            return await _context.Evenements.Include(e => e.Commentaires).Skip((page - 1) * pagesize).Take(pagesize).ToListAsync();
        }

        //return specific Event with comments
        [HttpGet("{id}")]
        public async Task<ActionResult<Evenement>> GetEvent(long id)
        {
           var Event = await  _context.Evenements
                         .Where(x => x.Id == id)
                         .Include(x => x.Commentaires)
                         .FirstOrDefaultAsync();

            if (Event == null)
            {
                return NotFound();
            }

            return Event;
        }

        //Add new Event
        [HttpPost]
        public async Task<ActionResult<Evenement>> AddEvent(string titre, string description, string nompresonne, List<Commentaire> commentaires)
        {
            var Evenement = new Evenement
            {
                Titre = titre,
                Description = description,
                NomPersonne = nompresonne,
                Commentaires = commentaires
            };

            _context.Evenements.Add(Evenement);
            await _context.SaveChangesAsync();

            //return http 201 status code if successful
            return CreatedAtAction(nameof(Evenement), new { id = Evenement.Id }, Evenement);
        }

        //Update Event
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(long id, Evenement Event)
        {
            if (id != Event.Id)
            {
                return BadRequest();
            }

            _context.Entry(entity: Event).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EvenementItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        //Delete a specefic Event
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(long id)
        {
            var Event = await _context.Evenements.FindAsync(id);

            if (Event == null)
            {
                return NotFound();
            }

            _context.Evenements.Remove(Event);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        #region private_methods
        //check if Events exist in DB
        private bool EvenementItemExists(long id) =>
     _context.Evenements.Any(e => e.Id == id);
        #endregion
    }
}
