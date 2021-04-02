using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
#nullable disable

namespace PoliceWebApplication
{
    public partial class City
    {
        public City()
        {
            Streets = new HashSet<Street>();
        }

        public int Id { get; set; }
        [Display(Name = "Назва міста")]
        [Required(ErrorMessage = "Введіть, будь ласка, назву міста")]
        [RegularExpression(@"^[Є-ЯҐ,а-їґ,`,',']{2,40}$",
         ErrorMessage = "Введіть, будь ласка, назву міста")]
        [Remote(action: "VerifyName", controller: "Cities")]
        public string Name { get; set; }
        public virtual ICollection<Street> Streets { get; set; }
    }
}
