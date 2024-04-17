using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;
using System.Text;
using SendGrid;
using SendGrid.Helpers.Mail;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace RocketApplication
{
    public partial class AppForm : Form
    {
        List<RocketModel> rockets = new List<RocketModel>();
        Dictionary<int, string> mapIdTime = new Dictionary<int, string>();
        Dictionary<int, string> mapIdStatus = new Dictionary<int, string>();
        System.Windows.Forms.Timer timer;
        bool sendEmailInSunday = true;
        DateTime limitDay = getNextWeekLastDay();

        // For testing
        bool oneTime = true;
        bool oneTime2 = true;
        bool testEmailWeekly = false;
        bool endTestingPhase = false;
        int testId1 = 0;
        int testId2 = 0;


        public AppForm()
        {
            InitializeComponent();
            LoadRockets();
            UpcomingRockets();
            Timer(10000, true, false); // 10 sec, call API
            Timer(30000, false, true); // 30 sec, update Labels
            Timer(15000, false, false); // 15 sec, check Sunday Email 
        }

        private void Timer(int ms, bool useApiTimer, bool useUpdateLabelsTimer)
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = ms;
            timer.Enabled = true;
            if (useApiTimer)
                timer.Tick += new EventHandler(apiTimer_Tick);
            else if (useUpdateLabelsTimer)
                timer.Tick += new EventHandler(updateLabels_Tick);
            else
                timer.Tick += new EventHandler(sendSundayEmail_Tick);

        }

        private void sendSundayEmail_Tick(object sender, EventArgs e)
        {
            sendSundayEmail();
        }
        private void updateLabels_Tick(object sender, EventArgs e)
        {
            UpdateLabels();
        }
        private void apiTimer_Tick(object sender, EventArgs e)
        {
            UpcomingRockets();
        }
        private void LoadRockets()
        {
            rockets = SQLiteDataAccess.LoadRockets();

            foreach (var rocket in rockets)
            {
                if (!mapIdTime.ContainsKey(rocket.Id))
                    mapIdTime.Add(rocket.Id, rocket.LaunchTime);
                if (!mapIdStatus.ContainsKey(rocket.Id))
                    mapIdStatus.Add(rocket.Id, rocket.LaunchStatus);
            }

            UpdateLabels();
            WireUpList();
        }

        private void WireUpList()
        {
            listDataBase.DataSource = null;
            listDataBase.DataSource = rockets;
            listDataBase.DisplayMember = "Rockets";
        }

        private async void labelTodayRocekts(string rocketName, string launchLocation, string launchTimeSlovenia, int rocketId, string launchStatus, string imageUrl)
        {
            string launchInfo = $"Rocket: {rocketName}, Launch Location: {launchLocation}, Launch Time: {launchTimeSlovenia}";

            if (launchStatus != "Launch Failure")
                labelRockets.Text += launchInfo + Environment.NewLine;

            RocketModel rocket = new RocketModel();

            rocket.RocketName = rocketName;
            rocket.LaunchLocation = launchLocation;
            rocket.LaunchTime = launchTimeSlovenia;
            rocket.Id = rocketId;
            rocket.ImageURL = imageUrl;

            SQLiteDataAccess.SaveRocket(rocket);
            mapIdTime.Add(rocketId, launchTimeSlovenia);
            mapIdStatus.Add(rocketId, launchStatus);
        }

        private async void labelNextRocekts(string rocketName, string launchLocation, string launchTimeSlovenia, int rocketId, string launchStatus, string imageUrl)
        {
            string launchInfo = $"Rocket: {rocketName}, Launch Location: {launchLocation}, Launch Time: {launchTimeSlovenia}";

            if (launchStatus != "Launch Failure")
                labelNextRockets.Text += launchInfo + Environment.NewLine;

            RocketModel rocket = new RocketModel();

            rocket.RocketName = rocketName;
            rocket.LaunchLocation = launchLocation;
            rocket.LaunchTime = launchTimeSlovenia;
            rocket.Id = rocketId;
            rocket.ImageURL = imageUrl;


            SQLiteDataAccess.SaveRocket(rocket);
            mapIdTime.Add(rocketId, launchTimeSlovenia);
            mapIdStatus.Add(rocketId, launchStatus);
        }
        private async Task UpcomingRockets()
        {
            try
            {             
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://lldev.thespacedevs.com/2.2.0/launch/upcoming/");
             //   HttpResponseMessage response = await client.GetAsync("https://ll.thespacedevs.com/2.2.0/launch/upcoming/");

                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(jsonString);

                    DateTime currentTimeUTC = DateTime.UtcNow;
                    TimeZoneInfo sloveniaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
                    DateTime currentTimeSlovenia = TimeZoneInfo.ConvertTimeFromUtc(currentTimeUTC, sloveniaTimeZone);

                    foreach (dynamic launch in data.results)
                    {
                        DateTime launchTime = DateTime.Parse((string)launch.net);
                        DateTime launchTimeSlovenia = TimeZoneInfo.ConvertTimeFromUtc(launchTime, sloveniaTimeZone);
                        TimeSpan timeUntilLaunch = launchTimeSlovenia - currentTimeSlovenia;

                        int rocketId = launch.rocket.id;
                        string rocketName = launch.rocket.configuration.name;
                        string launchLocation = launch.pad.location.name;
                        string imageUrl = launch.image;
                        string launchStatus = launch.status.name;
                        int hours = 24;

                        if (timeUntilLaunch.TotalHours <= hours && timeUntilLaunch.TotalHours >= 0 && !mapIdTime.ContainsKey(rocketId))
                        {
                            labelTodayRocekts(rocketName, launchLocation, launchTimeSlovenia.ToString(), rocketId, launchStatus, imageUrl);
                        }


                        else if (mapIdTime.ContainsKey(rocketId))
                        {

                            if (mapIdStatus[rocketId] != launchStatus && launchStatus == "Launch Failure")
                            {
                                listChanges.Items.Add($"{rocketName} changed launch sttus from: {mapIdStatus[rocketId]} to launch status: {launch}");
                                mapIdStatus[rocketId] = "Launch Failure";
                                UpdateLabelStatus(rocketId, launchStatus, currentTimeSlovenia);
                            }

                            else if (mapIdTime[rocketId] != launchTimeSlovenia.ToString() && endTestingPhase)
                            {
                                listChanges.Items.Add($"{rocketName} changed launch time from: {mapIdTime[rocketId]} to launch time: {launchTimeSlovenia.ToString()}");
                                mapIdTime[rocketId] = launchTimeSlovenia.ToString();
                                UpdateLabelTime(rocketId, launchTimeSlovenia.ToString(), currentTimeSlovenia);
                            }

                            // For testing
                            if (rocketId == testId1 && oneTime)
                            {
                                oneTime = false;
                                DateTime tempLaunch = launchTimeSlovenia.AddHours(1);
                                listChanges.Items.Add($"{launch.rocket.configuration.name} changed launch time from: {mapIdTime[rocketId]} to launch time: {tempLaunch.ToString()}");
                                mapIdTime[rocketId] = tempLaunch.ToString();
                                UpdateLabelTime(rocketId, tempLaunch.ToString(), currentTimeSlovenia);
                            }
                            if (rocketId == testId2 && oneTime2)
                            {
                                oneTime2 = false;
                                DateTime tempLaunch = launchTimeSlovenia.AddHours(1);
                                listChanges.Items.Add($"{launch.rocket.configuration.name} changed launch time from: {mapIdTime[rocketId]} to launch time: {tempLaunch.ToString()}");
                                mapIdTime[rocketId] = tempLaunch.ToString();
                                UpdateLabelTime(rocketId, tempLaunch.ToString(), currentTimeSlovenia);
                            }
                        }
                        if (timeUntilLaunch.TotalHours >= 0 && launchTimeSlovenia <= limitDay && !mapIdTime.ContainsKey(rocketId))
                        {
                            labelNextRocekts(rocketName, launchLocation, launchTimeSlovenia.ToString(), rocketId, launchStatus, imageUrl);
                        }
                    }
                    LoadRockets();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private async void UpdateLabelTime(int rocketId, string newLaunchTime, DateTime currentTimeSlovenia)
        {
            int hoursInDay = 24;
            labelRockets.Text = "";
            labelNextRockets.Text = "";
            foreach (dynamic rocket in rockets)
            {
                DateTime launchTimeSlovenia = DateTime.Parse(rocket.LaunchTime);
                TimeSpan timeUntilLaunch = launchTimeSlovenia - currentTimeSlovenia;


                string rocketName = rocket.RocketName;
                string launchLocation = rocket.LaunchLocation;
                string launchTime = rocket.LaunchTime;
                string imageUrl = rocket.ImageURL;

                if (rocket.Id == rocketId)
                {                
                    string launchInfo = $"Rocket: {rocketName}, Launch Location: {launchLocation}, Launch Time: {newLaunchTime}";
                    if (timeUntilLaunch.TotalHours <= hoursInDay && timeUntilLaunch.TotalHours >= 0)
                        labelRockets.Text += launchInfo + Environment.NewLine;
                    else if (timeUntilLaunch.TotalHours >= 0 && launchTimeSlovenia <= limitDay)
                        labelNextRockets.Text += launchInfo + Environment.NewLine;
                    rocket.LaunchTime = newLaunchTime;     
                    
                    SQLiteDataAccess.UpdateRocket(rocket);

                    string subject = $"{rocketName} changed Launch Time";

                    int width = 300;
                    int height = 200;

                    string htmlBody = "<html><body>" +
                    "<h1>IMPORTANT</h1>" +
                    $"<h3>{rocketName} on launch location: {launchLocation} changed launch time from: {launchTime} to launch time: {newLaunchTime}</h3>" +
                    $"<h3 style = \"color: red; \">Time until launch: {timeUntilLaunch.Hours}:{timeUntilLaunch.Minutes}:{timeUntilLaunch.Seconds}" +
                    $" Hours:Mins:Sec</h3>" +
                    $"<img src=\"{imageUrl}\" width = \"{width}\" height = \"{height}\">" +
                    "</body></html>";

                    string json = File.ReadAllText("EmailAdresses.json");
                    dynamic jsonData = JsonConvert.DeserializeObject(json);
                    foreach (string email in jsonData.EmailAdresses)
                    {
                        SendEmail(email, subject, htmlBody);
                    }

                }
                else
                {
                    string launchInfo = $"Rocket: {rocketName}, Launch Location: {launchLocation}, Launch Time: {launchTime}";
                    if (timeUntilLaunch.TotalHours <= hoursInDay && timeUntilLaunch.TotalHours >= 0)
                        labelRockets.Text += launchInfo + Environment.NewLine;
                    else if (timeUntilLaunch.TotalHours >= 0 && launchTimeSlovenia <= limitDay)
                        labelNextRockets.Text += launchInfo + Environment.NewLine;
                }
            }
            labelEmailSent.Text = "Warning email sent!";
        }

        private async void UpdateLabelStatus(int rocketId, string oldLaunchStatus, DateTime currentTimeSlovenia)
        {
            labelRockets.Text = "";
            labelNextRockets.Text = "";

            int hoursInDay = 24;
            string newLaunchStatus = "Launch Failure";

            int width = 300;
            int height = 200;

            foreach (dynamic rocket in rockets)
            {
                DateTime launchTimeSlovenia = DateTime.Parse(rocket.LaunchTime);
                TimeSpan timeUntilLaunch = launchTimeSlovenia - currentTimeSlovenia;


                string rocketName = rocket.RocketName;
                string launchLocation = rocket.LaunchLocation;
                string launchTime = rocket.LaunchTime;
                string imageUrl = rocket.ImageURL;
                if (rocket.Id == rocketId)
                {
                    SQLiteDataAccess.DeleteRocket(rocket);
                    string subject = $"{rocketName} changed launch status from {oldLaunchStatus} to STATUS: {newLaunchStatus}";

                    string htmlBody = "<html><body>" +
                               "<h1>IMPORTANT</h1>" +
                               $"<h3>{rocketName} on launch location: {launchLocation} changed launch status from: {oldLaunchStatus} to launch STATUS: {newLaunchStatus}<h3>" +
                                $"<img src=\"{imageUrl}\" width = \"{width}\" height = \"{height}\">" +
                                "</body></html>";

                    string json = File.ReadAllText("EmailAdresses.json");
                    dynamic jsonData = JsonConvert.DeserializeObject(json);
                    foreach (string email in jsonData.EmailAdresses)
                    {
                        SendEmail(email, subject, htmlBody);
                    }

                }
                else
                {
                    string launchInfo = $"Rocket: {rocketName}, Launch Location: {launchLocation}, Launch Time: {launchTime}";
                    if (timeUntilLaunch.TotalHours <= hoursInDay && timeUntilLaunch.TotalHours >= 0)
                        labelRockets.Text += launchInfo + Environment.NewLine;
                    else if (timeUntilLaunch.TotalHours >= 0 && launchTimeSlovenia <= limitDay)
                        labelNextRockets.Text += launchInfo + Environment.NewLine;
                }
            }
            labelEmailSent.Text = "Warning email sent!";
        }

        private async void UpdateLabels()
        {
            int hoursInDay = 24;
            labelRockets.Text = "";
            labelNextRockets.Text = "";
            labelEmailSent.Text = "";
            labelEmailPlannedRockets.Text = "";
            SQLiteDataAccess.DeleteExpiredRocket();

            foreach (dynamic rocket in rockets)
            {

                DateTime currentTimeUTC = DateTime.UtcNow;
                TimeZoneInfo sloveniaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
                DateTime currentTimeSlovenia = TimeZoneInfo.ConvertTimeFromUtc(currentTimeUTC, sloveniaTimeZone);

                DateTime launchTimeSlovenia = DateTime.Parse(rocket.LaunchTime);
                TimeSpan timeUntilLaunch = launchTimeSlovenia - currentTimeSlovenia;


                string rocketName = rocket.RocketName;
                string launchLocation = rocket.LaunchLocation;
                string launchTime = rocket.LaunchTime;

                string launchInfo = $"Rocket: {rocketName}, Launch Location: {launchLocation}, Launch Time: {launchTime}";
                if (timeUntilLaunch.TotalHours <= hoursInDay && timeUntilLaunch.TotalHours >= 0)
                    labelRockets.Text += launchInfo + Environment.NewLine;
                else if (timeUntilLaunch.TotalHours >= 0 && launchTimeSlovenia <= limitDay)
                    labelNextRockets.Text += launchInfo + Environment.NewLine;


            }
        }

        private async void sendSundayEmail()
        {
            DateTime currentTime = DateTime.Now;
            int width = 300;
            int height = 200;
            int hoursInDay = 24;

            if (currentTime.DayOfWeek == DayOfWeek.Monday) sendEmailInSunday = true;

            if (testEmailWeekly || currentTime.DayOfWeek == DayOfWeek.Sunday && currentTime.Hour == 6 && sendEmailInSunday)
            {
                string body = "";

                foreach (dynamic rocket in rockets)
                {
                    DateTime currentTimeUTC = DateTime.UtcNow;
                    TimeZoneInfo sloveniaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
                    DateTime currentTimeSlovenia = TimeZoneInfo.ConvertTimeFromUtc(currentTimeUTC, sloveniaTimeZone);

                    DateTime launchTimeSlovenia = DateTime.Parse(rocket.LaunchTime);
                    TimeSpan timeUntilLaunch = launchTimeSlovenia - currentTimeSlovenia;

                    string rocketName = rocket.RocketName;
                    string launchLocation = rocket.LaunchLocation;
                    string launchTime = rocket.LaunchTime;
                    string imageUrl = rocket.ImageURL;


                    if (timeUntilLaunch.TotalHours > hoursInDay && launchTimeSlovenia <= limitDay)
                    {

                        body += $"<h3>{rocketName} on launch location: {launchLocation} with launch time: {launchTime}</h3>" +
                                $"<h3 style = \"color: red; \">Time until launch: {timeUntilLaunch.Days}:{timeUntilLaunch.Hours}:{timeUntilLaunch.Minutes}:{timeUntilLaunch.Seconds}" +
                                $" Days:Hours:Mins:Sec</h3>" +
                                $"<img src=\"{imageUrl}\" width = \"{width}\" height = \"{height}\">" +
                                $"<br/><br/><hr><br/><br/>";
                    }
                }

                string htmlBody = "<html><body>" + body + "</body></html>";
                string subject = "New planned rockets!";

                string json = File.ReadAllText("EmailAdresses.json");
                dynamic jsonData = JsonConvert.DeserializeObject(json);
                foreach (string email in jsonData.EmailAdresses)
                {
                    SendEmail(email, subject, htmlBody);
                }

                sendEmailInSunday = false;
                testEmailWeekly = false;
                labelEmailPlannedRockets.Text = "Weekly email sent!";

            }
        }


        private async Task SendEmail(string recipientEmail, string messageSubject, string htmlBody)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress("testrocket022@gmail.com");
                message.To.Add(recipientEmail);

                message.Subject = messageSubject;

                message.Body = htmlBody;
                message.IsBodyHtml = true;

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");

                smtpClient.Port = 587;

                smtpClient.Credentials = new NetworkCredential("testrocket022@gmail.com", "fhwjdktwshoczuvo");
                smtpClient.EnableSsl = true;


                await smtpClient.SendMailAsync(message);


                // SendGrid way:
                  /* var apiKey = "YOUR_API_KEY";
                   var client = new SendGridClient(apiKey);
                   var from = new EmailAddress("testrocket022@gmail.com");
                   string subject = messageSubject;
                   var to = new EmailAddress(recipientEmail);
                   var msg = MailHelper.CreateSingleEmail(from, to, messageSubject, "", htmlBody);
                   var response = await client.SendEmailAsync(msg);
                */
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred: {ex.Message}");
            }

        }

        private static DateTime getNextWeekLastDay()
        {
            DateTime today = DateTime.Today;
            DateTime endOfWeek = today.AddDays(7 - (int)today.DayOfWeek);
            DateTime firstDayOfNextWeek = endOfWeek.AddDays(1);
            DateTime endOfNextWeek = firstDayOfNextWeek.AddDays(7 - (int)firstDayOfNextWeek.DayOfWeek);

            return endOfNextWeek;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          // SQLiteDataAccess.DeleteAllRockets();
          // listDataBase.DataSource = null;
        }

        private void testWarningEmail_Click(object sender, EventArgs e)
        {
            testId1 = 8164;
            testId2 = 8216;

        }

        private void testWeeklyEmail_Click(object sender, EventArgs e)
        {
            testEmailWeekly = true;
        }
    }
}
