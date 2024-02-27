using AddressBookWebApp.Context;
using AddressBookWebApp.Context.Requests;
using AddressBookWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AddressBookWebApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class HomeController : ControllerBase
    {

        private readonly INotesCollection _collection;
        private readonly IAddressBookDBContex _context;

        public HomeController(INotesCollection collection,
                              IAddressBookDBContex context)
        {
            _collection = collection;
            _context = context;
        }


 


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Note>>> GetByTheLetter(string letter, int page)
        {
            ByLetterData letterPage;

            if (letter == null || letter == "")
                letterPage = new ByLetterData("all", 0);
            else
                letterPage = new ByLetterData(letter,page);

            List<Note> notelist;
            try
            {
                notelist = await _collection.SelectListByLetter(_context, letterPage);               
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return Ok(notelist);
        }



        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Note>>> GetByTheClue(string clue, int page)
        {
            ByClueData cluePage;
            List<Note> notelist;

            if (clue != null || clue != "")
            {
                try
                {
                    cluePage = new ByClueData(clue, page);
                    notelist = await _collection.SelectListByClue(_context, cluePage);
                }
                catch (Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
                notelist = new List<Note>();

            return Ok(notelist);
        }



        [HttpPost]
        public async Task<IActionResult> AddNote(Note note)
        {
            try
            {
                await _collection.AddNote(_context, note);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return this.CreatedAtAction(nameof(this.AddNote), note);

        }


    }
}
