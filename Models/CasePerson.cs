using System;
using System.Collections.Generic;

#nullable disable

namespace PoliceWebApplication
{
    public partial class CasePerson
    {
        public int Id { get; set; }
        public int CaseId { get; set; }
        public int PersonId { get; set; }

        public virtual Case Case { get; set; }
        public virtual Person Person { get; set; }
    }
}
