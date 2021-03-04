using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string Name { get; set; }
        public virtual ICollection<Street> Streets { get; set; }
    }
}
