using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System;  // C#
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Data.SqlClient;

namespace MoodMeFinalUpload.Controllers
{
  
    public class HomeController : Controller
    {
        static List<Prediction> predictions = new List<Prediction>();
        public class StringTable
        {
            public string[] ColumnNames { get; set; }
            public string[,] Values { get; set; }
        }
        private class Prediction
        {
            public SqlBoolean Predicted_Good_Mood { get; set; }
            public SqlDouble Likelihood { get; set; }
            public SqlDateTime Date_Predicted { get; set; }
            public SqlDateTime Date { get; set; }
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public ActionResult Index(MyViewModel model)
        {
            EmptyTables();
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            String line ="";
            List<string> list = new List<string>();

            byte[] uploadedFile = new byte[model.File.InputStream.Length];
            model.File.InputStream.Read(uploadedFile, 0, uploadedFile.Length);
            String result = System.Text.Encoding.UTF8.GetString(uploadedFile);
            String temp = "";

            for(int i=0;i<result.Length;i++)
            {
                if (result[i] == '\n')
                {
                    list.Add(temp);
                    temp = "";
                    continue;
                }
                else
                {
                    temp += result[i];
                }
            }
            LoadSummaryTable(list);
            LoadDailyTable();
            RunStoredProc();
            PredictGoodMood();

            return Redirect("https://msit.powerbi.com/groups/me/dashboards/c116d6c1-0a49-455c-a726-fbd922562d50");
        }
        static void LoadDailyTable()
        {
            string line;
            FileStream aFile = new FileStream("C:\\Projects\\Daily.csv", FileMode.Open);
            StreamReader sr = new StreamReader(aFile);
            string DateBand = "";
            int Steps = 0;
            int Calories = 0;
            int HR_Lowest = 0;
            int HR_Highest = 0;
            int HR_Average = 0;
            float Total_Miles_Moved = 0;
            int Active_Hours = 0;
            int Total_Seconds_All_Activities = 0;
            int Total_Calories_All_Activities = 0;
            int Sleep_Events = 0;
            int Sleep_Total_Calories = 0;
            int Total_Seconds_Slept = 0;
            int Run_Events = 0;
            int Run_Total_Seconds = 0;
            float Total_Miles_Run = 0;
            int Run_Total_Calories = 0;
            int Bike_Events = 0;
            int Bike_Total_Seconds = 0;
            float Total_Miles_Biked = 0;
            int Bike_Total_Calories = 0;
            int Exercise_Events = 0;
            int Exercise_Total_Seconds = 0;
            int Exercise_Total_Calories = 0;
            int Guided_Workout_Events = 0;
            int Guided_Workout_Total_Seconds = 0;
            int Guided_Workout_Total_Calories = 0;
            int Golf_Events = 0;
            int Golf_Total_Seconds = 0;
            float Total_Miles_Golfed = 0;
            int Golf_Total_Calories = 0;
            // read data in line by line
            int ctr = 0;
            while ((line = sr.ReadLine()) != null)
            {
                Console.WriteLine(line);
                var values = line.Split(',');
                ctr++;
                Console.WriteLine(ctr);

                if (ctr == 1)
                    continue;
                if (values[0].Length > 2)
                    DateBand = values[0].Substring(1, values[0].Length - 2);
                Console.WriteLine(Steps);
                if (values[1].Length > 2)
                    Steps = Int32.Parse(values[1].Substring(1, values[1].Length - 2));
                if (values[2].Length > 2)
                    Calories = Int32.Parse(values[2].Substring(1, values[2].Length - 2));
                if (values[3].Length > 2)
                    HR_Lowest = Int32.Parse(values[3].Substring(1, values[3].Length - 2));
                if (values[4].Length > 2)
                    HR_Highest = Int32.Parse(values[4].Substring(1, values[4].Length - 2));
                if (values[5].Length > 2)
                    HR_Average = Int32.Parse(values[5].Substring(1, values[5].Length - 2));
                if (values[6].Length > 2)
                    Total_Miles_Moved = float.Parse(values[6].Substring(1, values[6].Length - 2));
                if (values[7].Length > 2)
                    Active_Hours = Int32.Parse(values[7].Substring(1, values[7].Length - 2));
                if (values[8].Length > 2)
                    Total_Seconds_All_Activities = Int32.Parse(values[8].Substring(1, values[8].Length - 2));
                if (values[9].Length > 2)
                    Total_Calories_All_Activities = Int32.Parse(values[9].Substring(1, values[9].Length - 2));
                if (values[10].Length > 2)
                    Sleep_Events = Int32.Parse(values[10].Substring(1, values[10].Length - 2));
                if (values[11].Length > 2)
                    Sleep_Total_Calories = Int32.Parse(values[11].Substring(1, values[11].Length - 2));
                if (values[12].Length > 2)
                    Total_Seconds_Slept = Int32.Parse(values[12].Substring(1, values[12].Length - 2));
                if (values[13].Length > 2)
                    Run_Events = Int32.Parse(values[13].Substring(1, values[13].Length - 2));
                if (values[14].Length > 2)
                    Run_Total_Seconds = Int32.Parse(values[14].Substring(1, values[14].Length - 2));
                if (values[15].Length > 2)
                    Total_Miles_Run = float.Parse(values[15].Substring(1, values[15].Length - 2));
                if (values[16].Length > 2)
                    Run_Total_Calories = Int32.Parse(values[16].Substring(1, values[16].Length - 2));
                if (values[17].Length > 2)
                    Bike_Events = Int32.Parse(values[17].Substring(1, values[17].Length - 2));
                if (values[18].Length > 2)
                    Bike_Total_Seconds = Int32.Parse(values[18].Substring(1, values[18].Length - 2));
                if (values[19].Length > 2)
                    Total_Miles_Biked = float.Parse(values[19].Substring(1, values[19].Length - 2));
                if (values[20].Length > 2)
                    Bike_Total_Calories = Int32.Parse(values[20].Substring(1, values[20].Length - 2));
                if (values[21].Length > 2)
                    Exercise_Events = Int32.Parse(values[21].Substring(1, values[21].Length - 2));
                if (values[22].Length > 2)
                    Exercise_Total_Seconds = Int32.Parse(values[22].Substring(1, values[22].Length - 2));
                if (values[23].Length > 2)
                    Exercise_Total_Calories = Int32.Parse(values[23].Substring(1, values[23].Length - 2));
                if (values[24].Length > 2)
                    Guided_Workout_Events = Int32.Parse(values[24].Substring(1, values[24].Length - 2));
                if (values[25].Length > 2)
                    Guided_Workout_Total_Seconds = Int32.Parse(values[25].Substring(1, values[25].Length - 2));
                if (values[26].Length > 2)
                    Guided_Workout_Total_Calories = Int32.Parse(values[26].Substring(1, values[26].Length - 2));
                if (values[27].Length > 2)
                    Golf_Events = Int32.Parse(values[27].Substring(1, values[27].Length - 2));
                if (values[28].Length > 2)
                    Golf_Total_Seconds = Int32.Parse(values[28].Substring(1, values[28].Length - 2));
                if (values[29].Length > 2)
                    Total_Miles_Golfed = float.Parse(values[29].Substring(1, values[29].Length - 2));
                if (values[30].Length > 2)
                    Golf_Total_Calories = Int32.Parse(values[30].Substring(1, values[30].Length - 2));
                string SQLConnectionString = "Server=tcp:csucla2015.database.windows.net,1433;Database=test1;User ID=meet_bhagdev@csucla2015;Password=avengersA1;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                // Create a SqlConnection from the provided connection string.
                using (SqlConnection connection = new SqlConnection(SQLConnectionString))
                {
                    // Begin to formulate the command.
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    // Specify the query to be executed.
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText =
                    @"INSERT INTO Daily (DateBand,Steps,Calories,HR_Lowest,HR_Highest,HR_Average,Total_Miles_Moved,Active_Hours,Total_Seconds_All_Activities,Total_Calories_All_Activities,Sleep_Events,Sleep_Total_Calories,Total_Seconds_Slept,Run_Events,Run_Total_Seconds,Total_Miles_Run,Run_Total_Calories,Bike_Events,Bike_Total_Seconds,Total_Miles_Biked,Bike_Total_Calories,Exercise_Events,Exercise_Total_Seconds,Exercise_Total_Calories,Guided_Workout_Events,Guided_Workout_Total_Seconds,Guided_Workout_Total_Calories,Golf_Events,Golf_Total_Seconds,Total_Miles_Golfed,Golf_Total_Calories) 
                VALUES (@DateBand,@Steps,@Calories,@HR_Lowest,@HR_Highest,@HR_Average,@Total_Miles_Moved,@Active_Hours,@Total_Seconds_All_Activities,@Total_Calories_All_Activities,@Sleep_Events,@Sleep_Total_Calories,@Total_Seconds_Slept,@Run_Events,@Run_Total_Seconds,@Total_Miles_Run,@Run_Total_Calories,@Bike_Events,@Bike_Total_Seconds,@Total_Miles_Biked,@Bike_Total_Calories,@Exercise_Events,@Exercise_Total_Seconds,@Exercise_Total_Calories,@Guided_Workout_Events,@Guided_Workout_Total_Seconds,@Guided_Workout_Total_Calories,@Golf_Events,@Golf_Total_Seconds,@Total_Miles_Golfed,@Golf_Total_Calories)";
                    command.Parameters.AddWithValue("@DateBand", DateBand);
                    command.Parameters.AddWithValue("@Steps", Steps);
                    command.Parameters.AddWithValue("@Calories", Calories);
                    command.Parameters.AddWithValue("@HR_Lowest", HR_Lowest);
                    command.Parameters.AddWithValue("@HR_Highest", HR_Highest);
                    command.Parameters.AddWithValue("@HR_Average", HR_Average);
                    command.Parameters.AddWithValue("@Total_Miles_Moved", Total_Miles_Moved);
                    command.Parameters.AddWithValue("@Active_Hours", Active_Hours);
                    command.Parameters.AddWithValue("@Total_Seconds_All_Activities", Total_Seconds_All_Activities);
                    command.Parameters.AddWithValue("@Total_Calories_All_Activities", Total_Calories_All_Activities);
                    command.Parameters.AddWithValue("@Sleep_Events", Sleep_Events);
                    command.Parameters.AddWithValue("@Sleep_Total_Calories", Sleep_Total_Calories);
                    command.Parameters.AddWithValue("@Total_Seconds_Slept", Total_Seconds_Slept);
                    command.Parameters.AddWithValue("@Run_Events", Run_Events);
                    command.Parameters.AddWithValue("@Run_Total_Seconds", Run_Total_Seconds);
                    command.Parameters.AddWithValue("@Total_Miles_Run", Total_Miles_Run);
                    command.Parameters.AddWithValue("@Run_Total_Calories", Run_Total_Calories);
                    command.Parameters.AddWithValue("@Bike_Events", Bike_Events);
                    command.Parameters.AddWithValue("@Bike_Total_Seconds", Bike_Total_Seconds);
                    command.Parameters.AddWithValue("@Total_Miles_Biked", Total_Miles_Biked);
                    command.Parameters.AddWithValue("@Bike_Total_Calories", Bike_Total_Calories);
                    command.Parameters.AddWithValue("@Exercise_Events", Exercise_Events);
                    command.Parameters.AddWithValue("@Exercise_Total_Seconds", Exercise_Total_Seconds);
                    command.Parameters.AddWithValue("@Exercise_Total_Calories", Exercise_Total_Calories);
                    command.Parameters.AddWithValue("@Guided_Workout_Events", Guided_Workout_Events);
                    command.Parameters.AddWithValue("@Guided_Workout_Total_Seconds", Guided_Workout_Total_Seconds);
                    command.Parameters.AddWithValue("@Guided_Workout_Total_Calories", Guided_Workout_Total_Calories);
                    command.Parameters.AddWithValue("@Golf_Events", Golf_Events);
                    command.Parameters.AddWithValue("@Golf_Total_Seconds", Golf_Total_Seconds);
                    command.Parameters.AddWithValue("@Total_Miles_Golfed", Total_Miles_Golfed);
                    command.Parameters.AddWithValue("@Golf_Total_Calories", Golf_Total_Calories);
                    // Open connection to database.
                    connection.Open();
                    // Read data from the query.
                    SqlDataReader reader = command.ExecuteReader();
                }
            }
            sr.Close();


        }

        static void EmptyTables()
        {
            using (var conn = new SqlConnection("Server=tcp:csucla2015.database.windows.net,1433;Database=test1;User ID=meet_bhagdev@csucla2015;Password=avengersA1;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    DELETE FROM Daily";
                conn.Open();
                cmd.ExecuteScalar();

                var cmd1 = conn.CreateCommand();
                cmd1.CommandText = @"
                    DELETE FROM Summary";
                cmd1.ExecuteScalar();
            }
        }
        static void LoadSummaryTable(List<string> list)
        {
            List<string> listString = list;

            string DateBand = "";
            string Start_Time = "";
            string Event_Type = "";
            int Duration_Second = 0;
            int Seconds_Paused = 0;
            int Calories_Burned = 0;
            int Calories_Burned_Carbs = 0;
            int Calories_Burned_Fats = 0;
            int HR_Lowest = 0;
            int HR_Peak = 0;
            int HR_Average = 0;
            double Total_Miles_Moved = 0;
            string Cardio_Benefit = "";
            int Minutes_Under_50_HR = 0;
            int Minutes_In_HRZ_Very_Light_50_60 = 0;
            int Minutes_In_HRZ_Light_60_70 = 0;
            int Minutes_In_HRZ_Moderate_70_80 = 0;
            int Minutes_In_HRZ_Hard_80_90 = 0;
            int Minutes_In_HRZ_Very_Hard_90_Plus = 0;
            int HR_Finish = 0;
            int HR_Recovery_Rate_1_Min = 0;
            int HR_Recovery_Rate_2_Min = 0;
            int Recovery_Time_Seconds = 0;
            int Bike_Average_MPH = 0;
            int Bike_Max_MPH = 0;
            int Elevation_Highest_Feet = 0;
            int Elevation_Lowest_Feet = 0;
            int Elevation_Gain_Feet = 0;
            int Elevation_Loss_Feet = 0;
            string Wake_Up_Time = "";
            int Seconds_Awake = 0;
            int Seconds_Asleep_Total = 0;
            int Seconds_Asleep_Restful = 0;
            int Seconds_Asleep_Light = 0;
            int Wake_Ups = 0;
            int Seconds_to_Fall_Asleep = 0;
            int Sleep_Efficiency = 0;
            string Sleep_Restoration = "";
            int Sleep_HR_Resting = 0;
            string Sleep_Auto_Detect = "";
            int GW_Plan_Name = 0;
            int GW_Reps_Performed = 0;
            int GW_Rounds_Performed = 0;
            int Golf_Course_Name = 0;
            int Golf_Course_Par = 0;
            int Golf_Total_Score = 0;
            int Golf_Par_or_Better = 0;
            int Golf_Pace_of_Play_Minutes = 0;
            int Golf_Longest_Drive_Yards = 0;

            string line;
            
            int ctr = 0;
            for (int i = 1; i < listString.Count; i++)
            {
                line = listString[i];
                Console.WriteLine(line);
                var values = line.Split(',');
                ctr++;
                Console.WriteLine(ctr);

                DateBand = values[0].Substring(1, values[0].Length - 2);
                Start_Time = values[1].Substring(1, values[1].Length - 2);
                Event_Type = values[2].Substring(1, values[2].Length - 2);

                Duration_Second = Int32.Parse(values[3].Substring(1, values[3].Length - 2));
                Duration_Second = Int32.Parse(values[3].Substring(1, values[3].Length - 2));
                Seconds_Paused = Int32.Parse(values[4].Substring(1, values[4].Length - 2));
                Calories_Burned = Int32.Parse(values[5].Substring(1, values[5].Length - 2));
                Console.WriteLine("Length" + values[6].Length);
                if (values[6].Length > 2)
                    Calories_Burned_Carbs = Int32.Parse(values[6].Substring(1, values[6].Length - 2));
                if (values[7].Length > 2)
                    Calories_Burned_Fats = Int32.Parse(values[7].Substring(1, values[7].Length - 2));
                if (values[8].Length > 2)
                    HR_Lowest = Int32.Parse(values[8].Substring(1, values[8].Length - 2));
                if (values[9].Length > 2)
                    HR_Peak = Int32.Parse(values[9].Substring(1, values[9].Length - 2));
                if (values[10].Length > 2)
                    HR_Average = Int32.Parse(values[10].Substring(1, values[10].Length - 2));
                if (values[11].Length > 2)
                    Total_Miles_Moved = double.Parse(values[11].Substring(1, values[11].Length - 2));
                Cardio_Benefit = values[12];
                if (values[13].Length > 2)
                    Minutes_Under_50_HR = Int32.Parse(values[13].Substring(1, values[13].Length - 2));
                if (values[14].Length > 2)
                    Minutes_In_HRZ_Very_Light_50_60 = Int32.Parse(values[14].Substring(1, values[14].Length - 2));
                if (values[15].Length > 2)
                    Minutes_In_HRZ_Light_60_70 = Int32.Parse(values[15].Substring(1, values[15].Length - 2));
                if (values[16].Length > 2)
                    Minutes_In_HRZ_Moderate_70_80 = Int32.Parse(values[16].Substring(1, values[16].Length - 2));
                if (values[17].Length > 2)
                    Minutes_In_HRZ_Hard_80_90 = Int32.Parse(values[17].Substring(1, values[17].Length - 2));
                if (values[18].Length > 2)
                    Minutes_In_HRZ_Very_Hard_90_Plus = Int32.Parse(values[18].Substring(1, values[18].Length - 2));
                if (values[19].Length > 2)
                    HR_Finish = Int32.Parse(values[19].Substring(1, values[19].Length - 2));
                if (values[20].Length > 2)
                    HR_Recovery_Rate_1_Min = Int32.Parse(values[20].Substring(1, values[20].Length - 2));
                if (values[21].Length > 2)
                    HR_Recovery_Rate_2_Min = Int32.Parse(values[21].Substring(1, values[21].Length - 2));
                if (values[22].Length > 2)
                    Recovery_Time_Seconds = Int32.Parse(values[22].Substring(1, values[22].Length - 2));
                if (values[23].Length > 2)
                    Bike_Average_MPH = Int32.Parse(values[23].Substring(1, values[23].Length - 2));
                if (values[24].Length > 2)
                    Bike_Max_MPH = Int32.Parse(values[24].Substring(1, values[24].Length - 2));
                if (values[25].Length > 2)
                    Elevation_Highest_Feet = Int32.Parse(values[25].Substring(1, values[25].Length - 2));
                if (values[26].Length > 2)
                    Elevation_Lowest_Feet = Int32.Parse(values[26].Substring(1, values[26].Length - 2));
                if (values[27].Length > 2)
                    Elevation_Gain_Feet = Int32.Parse(values[27].Substring(1, values[27].Length - 2));
                if (values[28].Length > 2)
                    Elevation_Loss_Feet = Int32.Parse(values[28].Substring(1, values[28].Length - 2));
                Wake_Up_Time = values[29].Substring(1, values[29].Length - 2);
                if (values[30].Length > 2)
                    Seconds_Awake = Int32.Parse(values[30].Substring(1, values[30].Length - 2));
                if (values[31].Length > 2)
                    Seconds_Asleep_Total = Int32.Parse(values[31].Substring(1, values[31].Length - 2));
                if (values[32].Length > 2)
                    Seconds_Asleep_Restful = Int32.Parse(values[32].Substring(1, values[32].Length - 2));
                if (values[33].Length > 2)
                    Seconds_Asleep_Light = Int32.Parse(values[33].Substring(1, values[33].Length - 2));
                if (values[34].Length > 2)
                    Wake_Ups = Int32.Parse(values[34].Substring(1, values[34].Length - 2));
                if (values[35].Length > 2)
                    Seconds_to_Fall_Asleep = Int32.Parse(values[35].Substring(1, values[35].Length - 2));
                if (values[36].Length > 2)
                    Sleep_Efficiency = Int32.Parse(values[36].Substring(1, values[36].Length - 2));
                Sleep_Restoration = values[37];
                if (values[38].Length > 2)
                    Sleep_HR_Resting = Int32.Parse(values[38].Substring(1, values[38].Length - 2));
                Sleep_Auto_Detect = values[39];
                if (values[40].Length > 2)
                    GW_Plan_Name = Int32.Parse(values[40].Substring(1, values[40].Length - 2));
                if (values[41].Length > 2)
                    GW_Reps_Performed = Int32.Parse(values[41].Substring(1, values[41].Length - 2));
                if (values[42].Length > 2)
                    GW_Rounds_Performed = Int32.Parse(values[42].Substring(1, values[42].Length - 2));
                if (values[43].Length > 2)
                    Golf_Course_Name = Int32.Parse(values[43].Substring(1, values[43].Length - 2));
                if (values[44].Length > 2)
                    Golf_Course_Par = Int32.Parse(values[44].Substring(1, values[44].Length - 2));
                if (values[45].Length > 2)
                    Golf_Total_Score = Int32.Parse(values[45].Substring(1, values[45].Length - 2));
                if (values[46].Length > 2)
                    Golf_Par_or_Better = Int32.Parse(values[46].Substring(1, values[46].Length - 2));
                if (values[47].Length > 2)
                    Golf_Pace_of_Play_Minutes = Int32.Parse(values[47].Substring(1, values[47].Length - 2));
                //if (values[48].Length > 2)
                  //  Golf_Longest_Drive_Yards = Int32.Parse(values[48].Substring(1, values[48].Length - 2));


                string SQLConnectionString = "Server=tcp:csucla2015.database.windows.net,1433;Database=test1;User ID=meet_bhagdev@csucla2015;Password=avengersA1;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                // Create a SqlConnection from the provided connection string.
                using (SqlConnection connection = new SqlConnection(SQLConnectionString))
                {
                    // Begin to formulate the command.
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    // Specify the query to be executed.
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText =
                    @"INSERT INTO Summary (DateBand,Start_Time,Event_Type,Duration_Second,Seconds_Paused,Calories_Burned,Calories_Burned_Carbs,Calories_Burned_Fats,HR_Lowest,HR_Peak,HR_Average,Total_Miles_Moved,Cardio_Benefit,Minutes_Under_50_HR,Minutes_In_HRZ_Very_Light_50_60,Minutes_In_HRZ_Light_60_70,Minutes_In_HRZ_Moderate_70_80,Minutes_In_HRZ_Hard_80_90,Minutes_In_HRZ_Very_Hard_90_Plus,HR_Finish,HR_Recovery_Rate_1_Min,HR_Recovery_Rate_2_Min,Recovery_Time_Seconds,Bike_Average_MPH,Bike_Max_MPH,Elevation_Highest_Feet,Elevation_Lowest_Feet,Elevation_Gain_Feet,Elevation_Loss_Feet,Wake_Up_Time,Seconds_Awake,Seconds_Asleep_Total,Seconds_Asleep_Restful,Seconds_Asleep_Light,Wake_Ups,Seconds_to_Fall_Asleep,Sleep_Efficiency,Sleep_Restoration,Sleep_HR_Resting,Sleep_Auto_Detect,GW_Plan_Name,GW_Reps_Performed,GW_Rounds_Performed,Golf_Course_Name,Golf_Course_Par,Golf_Total_Score,Golf_Par_or_Better,Golf_Pace_of_Play_Minutes,Golf_Longest_Drive_Yards) 
                VALUES (@DateBand,@Start_Time,@Event_Type,@Duration_Second,@Seconds_Paused,@Calories_Burned,@Calories_Burned_Carbs,@Calories_Burned_Fats,@HR_Lowest,@HR_Peak,@HR_Average,@Total_Miles_Moved,@Cardio_Benefit,@Minutes_Under_50_HR,@Minutes_In_HRZ_Very_Light_50_60,@Minutes_In_HRZ_Light_60_70,@Minutes_In_HRZ_Moderate_70_80,@Minutes_In_HRZ_Hard_80_90,@Minutes_In_HRZ_Very_Hard_90_Plus,@HR_Finish,@HR_Recovery_Rate_1_Min,@HR_Recovery_Rate_2_Min,@Recovery_Time_Seconds,@Bike_Average_MPH,@Bike_Max_MPH,@Elevation_Highest_Feet,@Elevation_Lowest_Feet,@Elevation_Gain_Feet,@Elevation_Loss_Feet,@Wake_Up_Time,@Seconds_Awake,@Seconds_Asleep_Total,@Seconds_Asleep_Restful,@Seconds_Asleep_Light,@Wake_Ups,@Seconds_to_Fall_Asleep,@Sleep_Efficiency,@Sleep_Restoration,@Sleep_HR_Resting,@Sleep_Auto_Detect,@GW_Plan_Name,@GW_Reps_Performed,@GW_Rounds_Performed,@Golf_Course_Name,@Golf_Course_Par,@Golf_Total_Score,@Golf_Par_or_Better,@Golf_Pace_of_Play_Minutes,@Golf_Longest_Drive_Yards)";
                    command.Parameters.AddWithValue("@DateBand", DateBand);
                    command.Parameters.AddWithValue("@Start_Time", Start_Time);
                    command.Parameters.AddWithValue("@Event_Type", Event_Type);
                    command.Parameters.AddWithValue("@Duration_Second", Duration_Second);
                    command.Parameters.AddWithValue("@Seconds_Paused", Seconds_Paused);
                    command.Parameters.AddWithValue("@Calories_Burned", Calories_Burned);
                    command.Parameters.AddWithValue("@Calories_Burned_Carbs", Calories_Burned_Carbs);
                    command.Parameters.AddWithValue("@Calories_Burned_Fats", Calories_Burned_Fats);
                    command.Parameters.AddWithValue("@HR_Lowest", HR_Lowest);
                    command.Parameters.AddWithValue("@HR_Peak", HR_Peak);
                    command.Parameters.AddWithValue("@HR_Average", HR_Average);
                    command.Parameters.AddWithValue("@Total_Miles_Moved", Total_Miles_Moved);
                    command.Parameters.AddWithValue("@Cardio_Benefit", Cardio_Benefit);
                    command.Parameters.AddWithValue("@Minutes_Under_50_HR", Minutes_Under_50_HR);
                    command.Parameters.AddWithValue("@Minutes_In_HRZ_Very_Light_50_60", Minutes_In_HRZ_Very_Light_50_60);
                    command.Parameters.AddWithValue("@Minutes_In_HRZ_Light_60_70", Minutes_In_HRZ_Light_60_70);
                    command.Parameters.AddWithValue("@Minutes_In_HRZ_Moderate_70_80", Minutes_In_HRZ_Moderate_70_80);
                    command.Parameters.AddWithValue("@Minutes_In_HRZ_Hard_80_90", Minutes_In_HRZ_Hard_80_90);
                    command.Parameters.AddWithValue("@Minutes_In_HRZ_Very_Hard_90_Plus", Minutes_In_HRZ_Very_Hard_90_Plus);
                    command.Parameters.AddWithValue("@HR_Finish", HR_Finish);
                    command.Parameters.AddWithValue("@HR_Recovery_Rate_1_Min", HR_Recovery_Rate_1_Min);
                    command.Parameters.AddWithValue("@HR_Recovery_Rate_2_Min", HR_Recovery_Rate_2_Min);
                    command.Parameters.AddWithValue("@Recovery_Time_Seconds", Recovery_Time_Seconds);
                    command.Parameters.AddWithValue("@Bike_Average_MPH", Bike_Average_MPH);
                    command.Parameters.AddWithValue("@Bike_Max_MPH", Bike_Max_MPH);
                    command.Parameters.AddWithValue("@Elevation_Highest_Feet", Elevation_Highest_Feet);
                    command.Parameters.AddWithValue("@Elevation_Lowest_Feet", Elevation_Lowest_Feet);
                    command.Parameters.AddWithValue("@Elevation_Gain_Feet", Elevation_Gain_Feet);
                    command.Parameters.AddWithValue("@Elevation_Loss_Feet", Elevation_Loss_Feet);
                    command.Parameters.AddWithValue("@Wake_Up_Time", Wake_Up_Time);
                    command.Parameters.AddWithValue("@Seconds_Awake", Seconds_Awake);
                    command.Parameters.AddWithValue("@Seconds_Asleep_Total", Seconds_Asleep_Total);
                    command.Parameters.AddWithValue("@Seconds_Asleep_Restful", Seconds_Asleep_Restful);
                    command.Parameters.AddWithValue("@Seconds_Asleep_Light", Seconds_Asleep_Light);
                    command.Parameters.AddWithValue("@Wake_Ups", Wake_Ups);
                    command.Parameters.AddWithValue("@Seconds_to_Fall_Asleep", Seconds_to_Fall_Asleep);
                    command.Parameters.AddWithValue("@Sleep_Efficiency", Sleep_Efficiency);
                    command.Parameters.AddWithValue("@Sleep_Restoration", Sleep_Restoration);
                    command.Parameters.AddWithValue("@Sleep_HR_Resting", Sleep_HR_Resting);
                    command.Parameters.AddWithValue("@Sleep_Auto_Detect", Sleep_Auto_Detect);
                    command.Parameters.AddWithValue("@GW_Plan_Name", GW_Plan_Name);
                    command.Parameters.AddWithValue("@GW_Reps_Performed", GW_Reps_Performed);
                    command.Parameters.AddWithValue("@GW_Rounds_Performed", GW_Rounds_Performed);
                    command.Parameters.AddWithValue("@Golf_Course_Name", Golf_Course_Name);
                    command.Parameters.AddWithValue("@Golf_Course_Par", Golf_Course_Par);
                    command.Parameters.AddWithValue("@Golf_Total_Score", Golf_Total_Score);
                    command.Parameters.AddWithValue("@Golf_Par_or_Better", Golf_Par_or_Better);
                    command.Parameters.AddWithValue("@Golf_Pace_of_Play_Minutes", Golf_Pace_of_Play_Minutes);
                    command.Parameters.AddWithValue("@Golf_Longest_Drive_Yards", Golf_Longest_Drive_Yards);
                    DateBand = "";
                    Start_Time = "";
                    Event_Type = "";
                    Duration_Second = 0;
                    Seconds_Paused = 0;
                    Calories_Burned = 0;
                    Calories_Burned_Carbs = 0;
                    Calories_Burned_Fats = 0;
                    HR_Lowest = 0;
                    HR_Peak = 0;
                    HR_Average = 0;
                    Total_Miles_Moved = 0;
                    Cardio_Benefit = "";
                    Minutes_Under_50_HR = 0;
                    Minutes_In_HRZ_Very_Light_50_60 = 0;
                    Minutes_In_HRZ_Light_60_70 = 0;
                    Minutes_In_HRZ_Moderate_70_80 = 0;
                    Minutes_In_HRZ_Hard_80_90 = 0;
                    Minutes_In_HRZ_Very_Hard_90_Plus = 0;
                    HR_Finish = 0;
                    HR_Recovery_Rate_1_Min = 0;
                    HR_Recovery_Rate_2_Min = 0;
                    Recovery_Time_Seconds = 0;
                    Bike_Average_MPH = 0;
                    Bike_Max_MPH = 0;
                    Elevation_Highest_Feet = 0;
                    Elevation_Lowest_Feet = 0;
                    Elevation_Gain_Feet = 0;
                    Elevation_Loss_Feet = 0;
                    Wake_Up_Time = "";
                    Seconds_Awake = 0;
                    Seconds_Asleep_Total = 0;
                    Seconds_Asleep_Restful = 0;
                    Seconds_Asleep_Light = 0;
                    Wake_Ups = 0;
                    Seconds_to_Fall_Asleep = 0;
                    Sleep_Efficiency = 0;
                    Sleep_Restoration = "";
                    Sleep_HR_Resting = 0;
                    Sleep_Auto_Detect = "";
                    GW_Plan_Name = 0;
                    GW_Reps_Performed = 0;
                    GW_Rounds_Performed = 0;
                    Golf_Course_Name = 0;
                    Golf_Course_Par = 0;
                    Golf_Total_Score = 0;
                    Golf_Par_or_Better = 0;
                    Golf_Pace_of_Play_Minutes = 0;
                    Golf_Longest_Drive_Yards = 0;
                    // Open connection to database.
                    connection.Open();
                    // Read data from the query.
                    SqlDataReader reader = command.ExecuteReader();
                }

            }
        }

        static void RunStoredProc() 
        {
            string SQLConnectionString = "Server=tcp:csucla2015.database.windows.net,1433;Database=test1;User ID=meet_bhagdev@csucla2015;Password=avengersA1;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (var conn = new SqlConnection(SQLConnectionString))
            using (var command = new SqlCommand("dbo.uspUpsertDailyActivity", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
        }
        static void PredictGoodMood()
        {
            using (var conn = new SqlConnection("Server=tcp:csucla2015.database.windows.net,1433;Database=test1;User ID=meet_bhagdev@csucla2015;Password=avengersA1;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                // Only get records that have not got prediction yet. 
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                        SELECT 
	                         [Date]	                -- 0				
	                        ,[Steps]				-- 1	
	                        ,[Calories]				-- 2
	                        ,[HR_Lowest]			-- 3	
	                        ,[HR_Highest]			-- 4
	                        ,[HR_Average]			-- 5
	                        ,[Hours_Slept]			-- 6
	                        ,[Total_Miles_Moved]	-- 7	
	                        ,[Wake_Up_Hour]			-- 8
	                        ,[Wake_Ups]				-- 9
	                        ,[Day_Name]				-- 10
	                        ,[Actual_Good_Mood]
                        FROM [dbo].[DailyActivity]
                        WHERE [Predicted_Good_Mood] IS NULL;";

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        //Console.WriteLine("ID: {0} Name: {1} Order Count: {2}", reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2));
                        DateTime Date = reader.GetDateTime(0);
                        int Steps = reader.GetInt32(1);
                        int Calories = reader.GetInt16(2);
                        int HR_Lowest = reader.GetInt16(3);
                        int HR_Highest = reader.GetInt16(4);
                        int HR_Average = reader.GetInt16(5);
                        double Hours_Slept = reader.GetDouble(6);
                        double Total_Miles_Moved = reader.GetDouble(7);
                        int Wake_Up_Hour = reader.GetInt32(8);
                        int Wake_Ups = reader.GetInt16(9);
                        string Day_Name = reader.GetString(10);



                        var scoreRequest = new
                        {

                            Inputs = new Dictionary<string, StringTable>() { 
                                    { 
                                        "input1", 
                                        new StringTable() 
                                        {
                                            ColumnNames = new string[] {"Steps", "Calories", "HR_Lowest", "HR_Highest", "HR_Average", "Hours_Slept", "Total_Miles_Moved", "Wake_Up_Hour", "Wake_Ups", "Day_Name", "Actual_Good_Mood"},
                                            Values = new string[,] {  {   Convert.ToString(Steps), 
                                                                          Convert.ToString(Calories), 
                                                                          Convert.ToString(HR_Lowest), 
                                                                          Convert.ToString(HR_Highest), 
                                                                          Convert.ToString(HR_Average), 
                                                                          Convert.ToString(Hours_Slept), 
                                                                          Convert.ToString(Total_Miles_Moved), 
                                                                          Convert.ToString(Wake_Up_Hour),
                                                                          Convert.ToString(Wake_Ups),
                                                                          Day_Name,
                                                                          "0"},  
                                                                      {   Convert.ToString(Steps), 
                                                                          Convert.ToString(Calories), 
                                                                          Convert.ToString(HR_Lowest), 
                                                                          Convert.ToString(HR_Highest), 
                                                                          Convert.ToString(HR_Average), 
                                                                          Convert.ToString(Hours_Slept), 
                                                                          Convert.ToString(Total_Miles_Moved), 
                                                                          Convert.ToString(Wake_Up_Hour),
                                                                          Convert.ToString(Wake_Ups),
                                                                          Day_Name,
                                                                          "0"},  }
                                        }
                                    },
                                                    },
                            GlobalParameters = new Dictionary<string, string>()
                            {
                            }
                        };

                        InvokeRequestResponseService(scoreRequest, Date).Wait();

                    }
                }
                conn.Close();

                // Update the table with the Predicted values.
                foreach (Prediction prediction in predictions)
                {
                    cmd = conn.CreateCommand();
                    cmd.CommandText = @"
                        UPDATE [dbo].[DailyActivity]
                        SET [Predicted_Good_Mood] = @Predicted
                            ,[Probability_Good_Mood] = @Probability
                            ,[Date_Predicted] = GETDATE()
                        WHERE [Date] = @Date";

                    cmd.Parameters.AddWithValue("@Predicted", prediction.Predicted_Good_Mood);
                    cmd.Parameters.AddWithValue("@Probability", prediction.Likelihood);
                    cmd.Parameters.AddWithValue("@Date", prediction.Date);

                    conn.Open();
                    cmd.ExecuteScalar();
                    conn.Close();
                }
            }

        }

        static async Task InvokeRequestResponseService(object scoreRequest, DateTime Date)
        {
            using (var client = new HttpClient())
            {
                const string apiKey = "7u/KKAolEGLuQDs9NNoLfnm78wPADM4yp7u9bXooLOW/2gIENjZDPeNlnP+cB6irJnKxlN6W7PGbl4f1N8oHmg=="; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                Console.WriteLine("HERE");
                client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/f481f916922040929aabb4ed24054120/services/546ebb3c981440b395d6d350970f5824/execute?api-version=2.0&details=true");
                HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    Console.WriteLine("HERE1");
                    Prediction a = new Prediction();
                    a.Date = Date;
                    a.Date_Predicted = DateTime.Now;

                    // parse the json output from AzureML
                    int i = result.IndexOf("True");
                    if (i == -1)
                        i = result.IndexOf("False");
                    string temp = result.Substring(i, result.Length - i);
                    i = temp.IndexOf(",");
                    String mood = temp.Substring(0, i - 1);
                    Console.WriteLine(mood);
                    int j = temp.IndexOf("0");
                    int k = temp.IndexOf("]");
                    string value1 = temp.Substring(j, k - j - 1);
                    double value = double.Parse(value1);
                    Console.WriteLine(value);

                    // get the Mood value and probability
                    a.Predicted_Good_Mood = Convert.ToBoolean(mood.ToLower());
                    a.Likelihood = value;

                    // add it to the array for update later
                    predictions.Add(a);

                }
                else
                {
                    Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                    // Print the headers - they include the requert ID and the timestamp, which are useful for debugging the failure
                    Console.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    Console.WriteLine(responseContent);
                }
            }
        }

    }


}