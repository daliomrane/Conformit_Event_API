using System;
using System.ComponentModel.DataAnnotations;

namespace TestProgrammationConformit.Entities
{
    public class Commentaire
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        [Required]
        public Evenement ParentEvent { get; set; }
    }
}