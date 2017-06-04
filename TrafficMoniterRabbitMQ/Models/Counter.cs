using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficMoniterRabbitMQ.Models
{
    public class Counter
    {
        public int Rank { get; set; }
        public string PS_ID { get; set; }
        public double Measure_m { get; set; }

        public static List<Counter> GetAllCounters()
        {
            return new List<Counter>()
            {
                new Counter() { PS_ID = "PS-697025" , Measure_m = 2332.22, Rank = 1 },
                new Counter() { PS_ID =  "PS-680514", Measure_m = 4994.75, Rank = 2 },
                new Counter() { PS_ID = "PS-697021", Measure_m =7383.34 , Rank = 3},
                new Counter() { PS_ID =  "PS-678451", Measure_m =9819.74, Rank = 4},
                new Counter() { PS_ID =  "PS-680384", Measure_m =12146.07, Rank = 5},
                new Counter() { PS_ID =  "PS-679343", Measure_m =27776.70, Rank = 6},
                new Counter() { PS_ID =  "PS-678457", Measure_m =33782.48, Rank = 7},
                new Counter() { PS_ID =  "PS-676713", Measure_m =39434.95, Rank = 8},
                new Counter() { PS_ID =  "PS-679347", Measure_m =41913.88, Rank = 9},
                new Counter() { PS_ID =  "PS-690198", Measure_m = 53505.24, Rank = 10},
                new Counter() { PS_ID =  "PS-680512", Measure_m = 60446.20, Rank = 11},
                new Counter() { PS_ID =  "PS-676692", Measure_m = 68755.24, Rank = 12}
            };
        }
    }
}
