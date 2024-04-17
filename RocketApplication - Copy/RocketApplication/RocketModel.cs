using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketApplication
{
    public class RocketModel
    {
        public int Id { get; set; }
        public string RocketName { get; set; }
       
        public string LaunchTime { get; set; }

        public string LaunchLocation { get; set; }

        public string ImageURL { get; set; }

        public string AnyChange { get; set; }

        public string WeeklyEmailDate { get; set; }

        public string LaunchStatus { get; set; }

        public string Rockets
        {
            get
            {
                return $"{Id} | {RocketName} | {LaunchTime} | {LaunchLocation} | {AnyChange} ";
             }
        }
    }
}
