using AddressBookWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AddressBookWebApp.Context.Requests
{

    public interface INotesCollection
    {
        Task AddNote(IAddressBookDBContex _context, Note note);
        Task ChangeNote(IAddressBookDBContex _context, int id, Note note);
        Task DeleteNote(IAddressBookDBContex _context, int id);
        Note SearchNote(IAddressBookDBContex _context, int id);
        Task<List<Note>> SelectListByLetter(IAddressBookDBContex _context, ByLetterData page);
        Task<List<Note>> SelectListByClue(IAddressBookDBContex _context, ByClueData page);

    }


    public class NotesCollection : INotesCollection
    {
        public NotesCollection()
        { }

        public async Task AddNote(IAddressBookDBContex context, Note note)
        {
            try
            {
                context.Note.Add(note);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new Exception("не удалось добавить строку");
            }

        }



        public async Task ChangeNote(IAddressBookDBContex context, int id, Note note)
        {
            var desired = context.Note.
                Where(n => n.Id == id).
                FirstOrDefault();

            if (desired != null)
            {
                try
                {
                    desired.FamilyName = note.FamilyName;
                    desired.Name = note.Name;
                    desired.PatronymicName = note.PatronymicName;
                    desired.Tel = note.Tel;
                    desired.Address = note.Address;
                    desired.Description = note.Description;
                    context.Note.Update(desired);
                    await context.SaveChangesAsync();
                }
                catch
                {
                    throw new Exception("не удалось изменить данные");
                }
            }
            else throw new Exception("требуемая для изменения строка не найдена");
        }




        public async Task DeleteNote(IAddressBookDBContex context, int id)
        {
            var desired = context.Note.
                 Where(n => (n.Id == id)).
                 FirstOrDefault();
            if (desired != null)
            {
                try
                {
                    context.Note.Remove(desired);
                    await context.SaveChangesAsync();
                }
                catch
                {
                    throw new Exception("не удалось удалить данные");
                }
            }
            else throw new Exception("требуемая для удаления строка не найдена");
        }



        public Note SearchNote(IAddressBookDBContex context, int id)
        {
            var desired = context.Note.
            Where(n => n.Id == id).
            FirstOrDefault();

            if (desired != null)
            {
                return desired;
            }
            else throw new Exception("требуемая строка не найдена в бд");
        }


        public async Task<List<Note>> SelectListByLetter(IAddressBookDBContex context, ByLetterData page)
        {
            if (string.Compare(page.Letter, "all") == 0)
                return await context.Note.Skip(page.Page * 8).Take(8).ToListAsync();

            return await context.Note.
                Where(n => EF.Functions.Like(n.FamilyName!, page.Letter + "%")).
                Skip(page.Page * 8).Take(8).ToListAsync();
        }


        

        public async Task<List<Note>> SelectListByClue(IAddressBookDBContex context, ByClueData page)
        {
            return await context.Note.
                Where(n => EF.Functions.Like(n.FamilyName, "%" + page.Clue + "%") ||
                           EF.Functions.Like(n.Name, "%" + page.Clue + "%") ||
                           EF.Functions.Like(n.PatronymicName!, "%" + page.Clue + "%") ||
                           EF.Functions.Like(n.Tel, "%" + page.Clue + "%") ||
                           EF.Functions.Like(n.Address!, "%" + page.Clue + "%") ||
                           EF.Functions.Like(n.Description!, "%" + page.Clue + "%")).
                           Skip(page.Page * 8).Take(8).ToListAsync();
        }

    }



}
