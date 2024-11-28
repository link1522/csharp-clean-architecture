using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhiteLagoon.Domain.Entities
{
    public class VillaNumber
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Villa Number")]
        public int Villa_Number { get; set; }

        [ForeignKey("Valla")]
        [Display(Name = "Villa Id")]
        public int VillaId { get; set; }
        public Villa Villa { get; set; }

        [Display(Name = "Special Details")]
        public string? SpecialDetails { get; set; }
    }
}
