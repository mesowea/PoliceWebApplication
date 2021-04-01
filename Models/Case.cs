using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

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
        [Display(Name = "№ справи")]
        public int Id { get; set; }
        public int InvestigatorId { get; set; }
        [Display(Name = "Інформація по справі")]
        [Required(ErrorMessage = "Введіть, будь ласка, інформацію по справі")]
        public string Info { get; set; }
        [Display(Name = "Дата відкриття справи")]
        [Required(ErrorMessage = "Введіть, будь ласка, дату відкриття справи")]
        [Remote(action: "VerifyDateStarted", controller: "Cases")]
        public DateTime DateStarted { get; set; }
        [Display(Name = "Дата закриття справи")]
        [Remote(action: "VerifyDateFinished", controller: "Cases", AdditionalFields = nameof(DateStarted))]
        public DateTime? DateFinished { get; set; }

        public virtual Investigator Investigator { get; set; }
        public virtual ICollection<CaseArticle> CasesArticles { get; set; }
        public virtual ICollection<CasePerson> CasesPeople { get; set; }
    }
}
