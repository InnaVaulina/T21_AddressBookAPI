using System.ComponentModel.DataAnnotations;

namespace AddressBookWebApp.Models
{
    public class Note
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "Фамилия")]
        public string FamilyName { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Отчетсво")]
        public string? PatronymicName { get; set; }

        [Display(Name = "Телефон")]
        public string Tel { get; set; }

        [Display(Name = "Адрес")]
        public string? Address { get; set; }

        [Display(Name = "Информация")]
        public string? Description { get; set; }
    }

}
