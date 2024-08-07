using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nookenApp.Models
{
    [Table("TVODVOD")]
    public class TVodvod
    {
        public double? SHEROH { get; set; }
        public double? KPD { get; set; }
        public double? MAXQ { get; set; }
        public double? MINQ { get; set; }
        public double? SHIRB { get; set; }
        public double? KOEFL { get; set; }
        public double? KOEFP { get; set; }
        public double? UKLON { get; set; }
        public double? DLINA { get; set; }
        public double? POPRAVKA { get; set; }
        public short TYPEVODVOD { get; set; }
        public short? KODPOTREB { get; set; }
        public short? ALGRAS { get; set; }
        public short? MAXTIME { get; set; }
        public short? PRIORITET { get; set; }
        public short KODVODVOD { get; set; }
        public string NAMEVODVOD { get; set; }
        public short KONTUR { get; set; }
        public short KODVOD { get; set; }
        public short UROVEN { get; set; }
        public string BALANS { get; set; }
    }
}
