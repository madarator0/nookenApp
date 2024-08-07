using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nookenApp.Models
{
    [Table("TOBJ")]
    public class  TObj
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short KODOBJ { get; set; }

        public double KOEFSPEED { get; set; }
        public double OTMSET { get; set; }
        public double NULDAT { get; set; }
        public double MAXP { get; set; }
        public double MINP { get; set; }
        public double KOEFK { get; set; }
        public double KOEFB { get; set; }
        public double KOEFC { get; set; }
        public short KODVOD { get; set; }
        [Required]
        [StringLength(50)]
        public string NAMEOBJ { get; set; }
        public short TYPEOBJ { get; set; }
        public short KODPOKAZ { get; set; }
        public short ALGRASH { get; set; }
        public short KODDAT { get; set; }
        public short TYPEDAT { get; set; }
        public short KANAL { get; set; }
        public short PRIORITET { get; set; }
        [Required]
        [StringLength(1)]
        public string VKL { get; set; }
    }
}
