using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
#nullable disable

namespace PoliceWebApplication
{
    public partial class Person
    {
        public Person()
        {
            CasesPeople = new HashSet<CasePerson>();
        }
        public int Id { get; set; }
        [Display(Name = "Ім'я")]
        [Required(ErrorMessage = "Введіть, будь ласка, ім'я суб`єкта")]
        [RegularExpression(@"^[Є-ЯҐ,а-їґ,`,',']{2,40} [Є-ЯҐ,а-їґ,`,',']{2,40} [Є-ЯҐ,а-їґ,`,',']{2,40}$",
         ErrorMessage = "Введіть, будь ласка, ім'я слідчого")]
        public string Name { get; set; }
        [Display(Name = "Дата народження")]
        [Required(ErrorMessage = "Введіть, будь ласка, дату народження суб`єкта")]
        [Remote(action: "VerifyDOB", controller: "People")]
        public DateTime DateOfBirth { get; set; }
        [Display(Name = "Тип суб`єкту")]
        public int TypeId { get; set; }

        public virtual Type Type { get; set; }
        public virtual ICollection<CasePerson> CasesPeople { get; set; }
    }
}
