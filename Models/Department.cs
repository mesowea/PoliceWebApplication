using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace PoliceWebApplication
{
    public partial class Department
    {
        public Department()
        {
            Investigators = new HashSet<Investigator>();
        }

        public int Id { get; set; }
        [Display(Name = "Вулиця")]
        public int StreetId { get; set; }
        [Display(Name ="Будинок")]
        [Required(ErrorMessage = "Введіть, будь ласка, номер будинку")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Введіть, будь ласка, номер будинку")]
        public int House { get; set; }
        [Display(Name = "Вулиця")]
        public virtual Street Street { get; set; }
        public virtual ICollection<Investigator> Investigators { get; set; }
    }
}
