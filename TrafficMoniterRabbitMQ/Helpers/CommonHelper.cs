using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrafficMoniterRabbitMQ.Models;

namespace TrafficMoniterRabbitMQ.Helpers
{
    public class CommonHelper
    {
        #region Init
        private static CommonHelper _commonHelper = null;
        public static CommonHelper Instance
        {
            get
            {
                if (_commonHelper == null)
                    _commonHelper = new CommonHelper();

                return _commonHelper;
            }
        }

        private static List<Car> carsContainer = new List<Car>(); 

        #endregion

        public enum SpeedLimit
        {
            MaxSpeed = 145,
            Normal = 120,
            MinSpeed = 80
        }
        
        public List<Car> CarsContainer { get { return carsContainer; } }

        public void MoniterAcciedent()
        {
            Timer timer = new Timer(Callback, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        }


        private void Callback(object state)
        {
            var groupsWithPS = CarsContainer.GroupBy(a => a.PS_ID)
                                .Select(g => new {
                                    count = g.Count(),
                                    g.First().PS_ID,
                                    Rank = Counter.GetAllCounters()
                                                .First(b=>b.PS_ID == g.First().PS_ID).Rank
                                }).OrderBy(a=>a.Rank);
            
            int limitCarsCount = 30;
            int totalSum = 0;
            int counter = 1;
            List<string> PS_IDs = new List<string>();
            foreach (var each in groupsWithPS)
            {
                PS_IDs.Add(each.PS_ID);
                totalSum += each.count;

                if(counter % 2 == 0)
                {
                    RemoveCarsFromContainer(PS_IDs);
                    if (totalSum > limitCarsCount)
                    {
                        NotifyAccident(each.PS_ID, totalSum);
                    }

                    totalSum = 0;
                    PS_IDs = new List<string>();
                }

                if (groupsWithPS.Count() == counter)
                {
                    if (totalSum > limitCarsCount)
                    {
                        RemoveCarsFromContainer(PS_IDs);
                        NotifyAccident(each.PS_ID, totalSum);

                        totalSum = 0;
                        PS_IDs = new List<string>();
                    }
                }

                counter++;
            }
        }

        private void RemoveCarsFromContainer(List<string> pS_IDs)
        {
            foreach (var each in carsContainer.Where(a => pS_IDs.Contains(a.PS_ID)).ToList())
            {
                carsContainer.Remove(each);
            }
        }

        private void NotifyAccident(string pS_ID, int totalCars)
        {
            using (var client = new Client())
            {
                client.PublishMessage(JsonConvert.SerializeObject(new { PS_ID = pS_ID, CarsCount = totalCars }), "Accidents");
            }
        }
    }
}
