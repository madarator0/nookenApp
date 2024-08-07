using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace nookenApp.Helper
{
    public class MeasurementsModels
    {
        [Key]
        public int Id { get; set; }

        [Column("sensor_number")]
        public int SensorNumber { get; set; }

        [Column("name_obj")]
        public string NameObj { get; set; }

        [Column("urov_L")]
        public double UrovL { get; set; }

        [Column("reyka_H")]
        public double ReykaH { get; set; }

        [Column("rashod_Q")]
        public double RashodQ { get; set; }

        [Column("charge")]
        public double Charge { get; set; }

        [Column("signal")]
        public double Signal { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("synchron")]
        public int Synchron { get; set; }
    }
}

