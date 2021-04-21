using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestProgrammationConformit.Entities
{
    public class Evenement
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string Titre { get; set; }
        public string Description { get; set; }
        public string NomPersonne { get; set; }
        public List<Commentaire> Commentaires {get; set;}
    }
}