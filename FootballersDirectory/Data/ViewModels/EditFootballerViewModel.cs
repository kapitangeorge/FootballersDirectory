
using FootballersDirectory.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FootballersDirectory.Data.ViewModels
{
    public class EditFootballerViewModel
    {
       

        public int Id { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Пол")]
        public string Gender { get; set; }

        public string[] Genders = new[] { "Мужской", "Женский" };

        [Required]
        [Display(Name = "Дата рождения")]
        [DataType(DataType.Text)]
        public DateTime Birthday { get; set; }

        [Display(Name = "Название команды")]
        public string TeamName { get; set; }

        [Required]
        [Display(Name = "Страна")]
        public string Country { get; set; }


        [Display(Name = "Выбирите команду")]
        public string SelectTeam { get; set; }

        public IEnumerable<SelectListItem> Teams { get; set; }
    }
}
