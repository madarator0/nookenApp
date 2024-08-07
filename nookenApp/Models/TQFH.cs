using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nookenApp.Models
{
    public class TQFH
    {
        public float UROV { get; set; }
        public float RASHOD { get; set; }
        [Key]
        public short KODOBJ { get; set; }
    }
}
