namespace AddressBookWebApp.Context.Requests
{


    public class ByLetterData
    {
        string letter;
        int page;

        public string Letter { get { return letter; } }
        public int Page { get { return page; } }

        private readonly string[] letters = {"А", "Б", "В", "Г", "Д", "Е", "Ж", "З", "И", "К",
                "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Э", "Ю", "Я"};

        public ByLetterData(string letter, int page)
        {
            this.letter = letter;
            this.page = page;
        }

    }


    public class ByClueData
    {
        string clue;
        int page;

        public string Clue { get { return clue; } }
        public int Page { get { return page; } }

        
        public ByClueData(string clue, int page)
        {
            this.clue = clue;
            this.page = page;
        }

    }


}
