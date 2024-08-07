using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nookenApp.Models
{
    public class Tbalans
    {
        public int Id { get; set; }
        public byte KODVODVOD { get; set; }
        public byte TYPEVODVOD { get; set; }
        public DateTime DATATIME { get; set; }
        public double RASHOD { get; set; }
    }
}
