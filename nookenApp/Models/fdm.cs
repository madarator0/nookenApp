using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nookenApp.Models
{
    public class Qoprosv
    {
        [Key, Column(Order = 0)]
        public short KODVOD { get; set; }

        [Key, Column(Order = 1)]
        public short KODOBJ { get; set; }

        [MaxLength(50)]
        public string NAMEOBJ { get; set; }

        [MaxLength(1)]
        public string VKL { get; set; }

        public float? MAXP { get; set; }
        public float? MINP { get; set; }
        public float? OTMSET { get; set; }
        public float? KOEFC { get; set; }
        public float? KOEFK { get; set; }
        public float? KOEFB { get; set; }
        public float? KOEFSPEED { get; set; }
        public short? ALGRASH { get; set; }
        public short? KODPOKAZ { get; set; }
        public short? TYPEDAT { get; set; }
        public float? NULDAT { get; set; }
        public short? KANAL { get; set; }
        public short? PRIORITET { get; set; }
    }

    public class Qvodzatv
    {
        [Key, Column(Order = 0)]
        public short KODVOD { get; set; }

        [Key, Column(Order = 1)]
        public short KODOBJ { get; set; }

        [MaxLength(50)]
        public string NAMEOBJ { get; set; }

        [MaxLength(1)]
        public string VKL { get; set; }

        public float? MAXP { get; set; }
        public float? MINP { get; set; }
        public float? OTMSET { get; set; }
        public float? KOEFC { get; set; }
        public float? KOEFK { get; set; }
        public float? KOEFB { get; set; }
        public float? KOEFSPEED { get; set; }
        public short? ALGRASH { get; set; }
        public short? KODPOKAZ { get; set; }
        public short? TYPEDAT { get; set; }
        public short? KODDAT { get; set; }
        public short? PRIORITET { get; set; }
    }

    public class QzatvPiorit
    {
        [Key, Column(Order = 0)]
        public short KODVOD { get; set; }

        [Key, Column(Order = 1)]
        public short KODOBJ { get; set; }

        [MaxLength(50)]
        public string NAMEOBJ { get; set; }

        [MaxLength(1)]
        public string VKL { get; set; }

        public float? MAXP { get; set; }
        public float? MINP { get; set; }
        public float? OTMSET { get; set; }
        public float? KOEFC { get; set; }
        public float? KOEFK { get; set; }
        public float? KOEFB { get; set; }
        public float? KOEFSPEED { get; set; }
        public short? ALGRASH { get; set; }
        public short? KODPOKAZ { get; set; }
        public short? TYPEDAT { get; set; }
        public short? KODDAT { get; set; }
        public short? PRIORITET { get; set; }
    }

    public class Qtopertimeobj
    {
        [Key, Column(Order = 0)]
        public short KODOBJ { get; set; }

        [Key, Column(Order = 1)]
        public DateTime DATATIME { get; set; }

        public float? TI { get; set; }
        public short? KODPOKAZ { get; set; }
        public short? TC { get; set; }
    }

    public class Qvodvodu
    {
        [Key]
        public short KODVODVOD { get; set; }

        [MaxLength(50)]
        public string NAMEVODVOD { get; set; }

        [MaxLength(50)]
        public string BALANS { get; set; }

        public float? SHIRB { get; set; }
        public float? KOEFL { get; set; }
        public float? KOEFP { get; set; }
        public float? UKLON { get; set; }
        public float? DLINA { get; set; }
        public float? MAXQ { get; set; }
        public float? MINQ { get; set; }
        public float? POPRAVKA { get; set; }
        public float? SHEROH { get; set; }
        public float? KPD { get; set; }
        public short? TYPEVODVOD { get; set; }
        public short? KODPOTREB { get; set; }
        public short? ALGRAS { get; set; }
        public short? MAXTIME { get; set; }
        public short? PRIORITET { get; set; }
        public short? KONTUR { get; set; }
        public short? KODVOD { get; set; }
        public short? UROVEN { get; set; }
    }

    public class Qvodzatvor1
    {
        [Key, Column(Order = 0)]
        public short KODVOD { get; set; }

        [Key, Column(Order = 1)]
        public short KODOBJ { get; set; }

        [MaxLength(50)]
        public string NAMEOBJ { get; set; }

        [MaxLength(1)]
        public string VKL { get; set; }

        public float? MAXP { get; set; }
        public float? MINP { get; set; }
        public float? OTMSET { get; set; }
        public float? KOEFC { get; set; }
        public float? KOEFK { get; set; }
        public float? KOEFB { get; set; }
        public float? KOEFSPEED { get; set; }
        public short? ALGRASH { get; set; }
        public short? KODPOKAZ { get; set; }
        public short? TYPEDAT { get; set; }
        public short? KODDAT { get; set; }
        public short? PRIORITET { get; set; }
    }

    public class Qvodu
    {
        [Key]
        public short KODVOD { get; set; }

        [MaxLength(30)]
        public string NAMEVOD { get; set; }

        [MaxLength(1)]
        public string REGKOD { get; set; }

        public float? SHIRB { get; set; }
        public float? KOEFL { get; set; }
        public float? KOEFP { get; set; }
        public float? UKLON { get; set; }
        public float? DLINA { get; set; }
        public float? MAXQ { get; set; }
        public float? MINQ { get; set; }
        public float? POPRAVKA { get; set; }
        public float? SHEROH { get; set; }
        public float? KPD { get; set; }
        public short? TYPEVOD { get; set; }
        public short? KODPOTREB { get; set; }
        public short? ALGRAS { get; set; }
        public short? MAXTIME { get; set; }
        public short? PRIORITET { get; set; }
    }

    public class Qutvod
    {
        [Key, Column(Order = 0)]
        public short KODVOD { get; set; }

        [Key, Column(Order = 1)]
        public short KODTVOD { get; set; }

        [MaxLength(30)]
        public string NAMEVOD { get; set; }

        public short? INTERVAL { get; set; }
        public float? USTAVKA { get; set; }
        public float? DOPUSK { get; set; }
        public short? NUMNB { get; set; }
        public short? NUMVB { get; set; }
        public short? KONTUR { get; set; }
        public float? RASHFAKT { get; set; }
        public float? DELTA { get; set; }
        public DateTime? DATATIME { get; set; }

        [MaxLength(1)]
        public string REGKOD { get; set; }

        public float? TEHPOT { get; set; }
        public short? KODALG { get; set; }
        public short? PRIORITET { get; set; }
    }

    public class Qobjkonturv
    {
        [Key, Column(Order = 0)]
        public short KODOBJ { get; set; }

        [Key, Column(Order = 1)]
        public short KODVOD { get; set; }

        [MaxLength(50)]
        public string NAMEOBJ { get; set; }

        [MaxLength(30)]
        public string NAMEVOD { get; set; }

        [MaxLength(1)]
        public string VKL { get; set; }

        public float? KOEFC { get; set; }
        public float? KOEFK { get; set; }
        public float? KOEFB { get; set; }
        public float? KOEFSPEED { get; set; }
        public short? KODTVOD { get; set; }
        public short? INTERVAL { get; set; }
        public float? OTMSET { get; set; }
        public float? NULDAT { get; set; }
        public float? MAXP { get; set; }
        public float? MINP { get; set; }
        public short? KANAL { get; set; }
        public short? PRIORITET { get; set; }
        public DateTime? DATATIME { get; set; }
    }

    public class Qobjkonturv2
    {
        [Key, Column(Order = 0)]
        public short KODOBJ { get; set; }

        [Key, Column(Order = 1)]
        public short KODVOD { get; set; }

        [MaxLength(50)]
        public string NAMEOBJ { get; set; }

        [MaxLength(30)]
        public string NAMEVOD { get; set; }

        [MaxLength(1)]
        public string VKL { get; set; }

        public float? KOEFC { get; set; }
        public float? KOEFK { get; set; }
        public float? KOEFB { get; set; }
        public float? KOEFSPEED { get; set; }
        public short? KODTVOD { get; set; }
        public short? INTERVAL { get; set; }
        public float? OTMSET { get; set; }
        public float? NULDAT { get; set; }
        public float? MAXP { get; set; }
        public float? MINP { get; set; }
        public short? KANAL { get; set; }
        public short? PRIORITET { get; set; }
        public DateTime? DATATIME { get; set; }
    }

    public class Qtvod
    {
        [Key]
        public short KODVOD { get; set; }

        [MaxLength(30)]
        public string NAMEVOD { get; set; }

        public short? INTERVAL { get; set; }
        public float? USTAVKA { get; set; }
        public float? DOPUSK { get; set; }
        public short? NUMNB { get; set; }
        public short? NUMVB { get; set; }
        public short? KONTUR { get; set; }
        public float? RASHFAKT { get; set; }
        public float? DELTA { get; set; }
        public DateTime? DATATIME { get; set; }

        [MaxLength(1)]
        public string REGKOD { get; set; }

        public float? TEHPOT { get; set; }
        public short? KODALG { get; set; }
        public short? PRIORITET { get; set; }
    }

    public class Qtypevod
    {
        [Key]
        public short KODVOD { get; set; }

        [MaxLength(30)]
        public string NAMEVOD { get; set; }

        public short? INTERVAL { get; set; }
        public float? USTAVKA { get; set; }
        public float? DOPUSK { get; set; }
        public short? NUMNB { get; set; }
        public short? NUMVB { get; set; }
        public short? KONTUR { get; set; }
        public float? RASHFAKT { get; set; }
        public float? DELTA { get; set; }
        public DateTime? DATATIME { get; set; }

        [MaxLength(1)]
        public string REGKOD { get; set; }

        public float? TEHPOT { get; set; }
        public short? KODALG { get; set; }
        public short? PRIORITET { get; set; }
    }
}

