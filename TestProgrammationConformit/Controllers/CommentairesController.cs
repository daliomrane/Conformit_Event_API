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
    public class CommentairesController :BaseApiController
    {
        private readonly ConformitContext _context;
        public CommentairesController(ConformitContext context)
        {
            _context = context;
        }

        //return all comments for an event
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Commentaire>>> GetComment(long id)
        {
            return await _context.Commentaires.Where(comment => comment.ParentEvent.Id == id).ToListAsync();
        }

        //Add new Comment
        [HttpPost("{id}")]
        public async Task<ActionResult<Commentaire>> AddComment(Commentaire comment , long id)
        {
            var Event = await _context.Evenements.FindAsync(id);

            if (Event == null)
            {
                return NotFound();
            }

            comment.ParentEvent = Event;

            _context.Commentaires.Add(comment);
            await _context.SaveChangesAsync();

            //return http 201 status code if successful
            return CreatedAtAction(nameof(Commentaire), new { id = comment.Id }, comment);
        }

        //Update Comment
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(long id, Commentaire comment)
        {
            if (id != comment.Id)
            {
                return BadRequest();
            }

            _context.Entry(entity: comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentaireItemExists(id))
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


        //Delete a specefic comment
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(long id)
        {
            var comment = await _context.Commentaires.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            _context.Commentaires.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        #region private_methods
        //check if Comment exist in DB
        private bool CommentaireItemExists(long id) =>
     _context.Commentaires.Any(c => c.Id == id);
        #endregion

    }
}
