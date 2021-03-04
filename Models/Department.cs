using System;
using System.Collections.Generic;

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
        public int StreetId { get; set; }
        public int House { get; set; }

        public virtual Street Street { get; set; }
        public virtual ICollection<Investigator> Investigators { get; set; }
    }
}
