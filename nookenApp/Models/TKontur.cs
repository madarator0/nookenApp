using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nookenApp.Models
{
    [Table("TKONTUR")]
    public class TKontur
    {
        [Key]
        [Column("KONTUR")]
        public short Kontur { get; set; }

        [Required]
        [Column("MAXUPR")]
        public short MaxUpr { get; set; }

        [Required]
        [Column("NAMEKONTUR", TypeName = "char(50)")]
        [StringLength(50)]
        public string NameKontur { get; set; }
    }
}
