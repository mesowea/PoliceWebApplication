using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PoliceWebApplication
{
    public partial class CriminalArticle
    {
        public CriminalArticle()
        {
            CasesArticles = new HashSet<CaseArticle>();
        }

        public int Id { get; set; }
        [Display(Name = "№")]
        public string Name { get; set; }
        public string Info { get; set; }

        public virtual ICollection<CaseArticle> CasesArticles { get; set; }
    }
}
