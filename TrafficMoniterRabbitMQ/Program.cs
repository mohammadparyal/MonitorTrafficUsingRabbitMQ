using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrafficMoniterRabbitMQ.Helpers;
using TrafficMoniterRabbitMQ.Models;

namespace TrafficMoniterRabbitMQ
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var client = new Client())
            {
                Console.WriteLine("Waiting for messsages: ");
                client.ConsumerCountsQueue((model, ea) =>
                {
                    var body = ea.Body;
                    if (body.Length > 0)
                    {
                        var cars = JsonConvert.DeserializeObject<List<Car>>(Encoding.UTF8.GetString(body));
                        if (cars.Count > 0)
                        {
                            foreach (var car in cars)
                            {
                                if (CommonHelper.Instance.CarsContainer.Exists(c => c.PlateNumber == car.PlateNumber))
                                    CommonHelper.Instance.CarsContainer.Remove(car);


                                if (car.Speed > (int)CommonHelper.SpeedLimit.MaxSpeed)
                                {
                                    client.PublishMessage(JsonConvert.SerializeObject(car), "Alerts");
                                    continue;
                                }

                                if (car.Speed < (int)CommonHelper.SpeedLimit.MinSpeed)
                                {
                                    CommonHelper.Instance.CarsContainer.Add(car);
                                }
                            }
                        }
                    }
                });

                CommonHelper.Instance.MoniterAcciedent();

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }



        }

    }
}
