using AddressBookWebApp.Context.Requests;
using AddressBookWebApp.Context;
using Microsoft.AspNetCore.Mvc;
using AddressBookWebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace AddressBookWebApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class SingleController : ControllerBase
    {

        private readonly INotesCollection _collection;
        private readonly IAddressBookDBContex _context;

        public SingleController(INotesCollection collection,
                              IAddressBookDBContex context)
        {
            _collection = collection;
            _context = context;
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult<Note> SingleNote(int id)
        {
            if ( id < 0)
                return NotFound();

            Note note;

            try
            {
                note = _collection.SearchNote(_context, id);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return Ok(note);
        }


        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeNote(int id, Note note)
        {
            try
            {
                await _collection.ChangeNote(_context, id, note);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }


        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            try
            {
                await _collection.DeleteNote(_context, id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }




    }
}
