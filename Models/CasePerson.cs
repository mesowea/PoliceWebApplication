using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace PoliceWebApplication
{
    public partial class CasePerson
    {
        
        public int Id { get; set; }
        public int CaseId { get; set; }
        [Display(Name = "Особова справа")]
        public int PersonId { get; set; }

        public virtual Case Case { get; set; }
        [Display(Name = "Ім`я")]
        public virtual Person Person { get; set; }
    }
}
