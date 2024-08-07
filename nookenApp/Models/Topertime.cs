using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nookenApp.Models
{
    public class Topertime
    {
        [Key]
        public int KODOBJ { get; set; }
        public int KODPOKAZ { get; set; }
        public DateTime DATATIME { get; set; }
        public double TI { get; set; }
        public int TC { get; set; }
    }
}
