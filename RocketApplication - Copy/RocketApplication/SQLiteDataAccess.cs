using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketApplication
{
    public class SQLiteDataAccess
    {

        public static List<RocketModel> LoadRockets() 
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionsString()))
            {
                var output = cnn.Query<RocketModel>("select * from Rocket", new DynamicParameters());
                return output.ToList();
            } 
        }


        public static void SaveRocket(RocketModel model)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionsString()))
            {
                cnn.Execute("insert into Rocket (RocketName, LaunchLocation, LaunchTime, Id, ImageURL, AnyChange, LaunchStatus) " +
                    "values (@RocketName, @LaunchLocation, @LaunchTime, @Id, @ImageURL, @AnyChange, @LaunchStatus)", model);
            }
        }

        private static string LoadConnectionsString(string Id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[Id].ConnectionString;
        }
        public static void DeleteAllRockets()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionsString()))
            {
                cnn.Execute("delete from Rocket");
            }
        }

        public static void DeleteRocket(RocketModel model)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionsString()))
            {
                cnn.Execute("delete from Rocket where Id = @Id", model);
            }
        }

        public static void SaveWeeklyEmailDate(RocketModel model)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionsString()))
            {
                cnn.Execute("UPDATE Rocket SET RocketName = @RocketName, LaunchLocation = @LaunchLocation, LaunchTime = @LaunchTime" +
                    ", ImageURL = @ImageURL, WeeklyEmailDate = @WeeklyEmailDate, LaunchStatus = @LaunchStatus WHERE Id = @Id", model);
            }
        }


        public static void DeleteExpiredRocket()
        {
            DateTime currentTime = DateTime.Today;
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionsString()))
            {
                var rockets = cnn.Query<RocketModel>("select * from Rocket", new DynamicParameters());
                
                foreach (var rocket in rockets)
                {
                    DateTime launchTime = DateTime.Parse(rocket.LaunchTime);
                    if(launchTime <= currentTime)
                    {
                        cnn.Execute("delete from Rocket where Id = @Id", rocket);
                    }
                }
            }
        }

    }

}
