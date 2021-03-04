using System;
using System.Collections.Generic;

#nullable disable

namespace PoliceWebApplication
{
    public partial class Case
    {
        public Case()
        {
            CasesArticles = new HashSet<CaseArticle>();
            CasesPeople = new HashSet<CasePerson>();
        }

        public int Id { get; set; }
        public int InvestigatorId { get; set; }
        public string Info { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime? DateFinished { get; set; }

        public virtual Investigator Investigator { get; set; }
        public virtual ICollection<CaseArticle> CasesArticles { get; set; }
        public virtual ICollection<CasePerson> CasesPeople { get; set; }
    }
}
