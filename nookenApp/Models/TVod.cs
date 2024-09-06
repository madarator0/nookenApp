using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nookenApp.Models
{
    [Table("TVOD")]
    public class TVod
    {
        [Key]
        [Column("DATATIME")]
        public DateTime Datatime { get; set; }

        [Column("USTAVKA", TypeName = "float")]
        public double Ustavka { get; set; }

        [Column("DOPUSK", TypeName = "float")]
        public double Dopuska { get; set; }

        [Column("KD", TypeName = "float")]
        public double Kd { get; set; }

        [Column("RASHFAKT", TypeName = "float")]
        public double Rashfakt { get; set; }

        [Column("KI", TypeName = "float")]
        public double Ki { get; set; }

        [Column("KP", TypeName = "float")]
        public double Kp { get; set; }

        [Column("DELTA", TypeName = "float")]
        public double Delta { get; set; }

        [Column("TEHPOT", TypeName = "float")]
        public double Tehpot { get; set; }

        [Required]
        [Column("UPRAV", TypeName = "char(20)")]
        public string Uprav { get; set; }

        [Column("INTERVAL")]
        public short Interval { get; set; }

        [Required]
        [Column("NAMETVOD", TypeName = "char(30)")]
        [StringLength(30)]
        public string NameTvod { get; set; }

        [Column("NUMNB")]
        public short NumNb { get; set; }

        [Column("NUMVB")]
        public short NumVb { get; set; }

        [Column("KONTUR")]
        public short Kontur { get; set; }

        [Column("KODALG")]
        public short KodAlg { get; set; }

        [Column("PRIORITET")]
        public short Prioritet { get; set; }

        [Column("KODVOD")]
        public short KodVod { get; set; }

        [Required]
        [Column("NAMEVOD", TypeName = "char(30)")]
        [StringLength(30)]
        public string NameVod { get; set; }

        [Column("KODTVOD")]
        public short KodTvod { get; set; }

        [Column("ITIME")]
        public short ITime { get; set; }

        [Column("DTIME")]
        public short DTime { get; set; }

        [Required]
        [Column("REGVKL", TypeName = "char(1)")]
        public string RegVkl { get; set; }
    }
}
