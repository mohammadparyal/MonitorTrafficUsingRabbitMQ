using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficMoniterRabbitMQ.Models
{
    public class Car
    {
        public string PS_ID { get; set; }
        public int Speed { get; set; }
        public String PlateNumber { get; set; }
        public DateTime EntryTime { get; set; }
    }
}
