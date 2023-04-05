using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class CPHInline
{
    public bool Execute()
    {
        //Month List
        string[] monthList = {"Januar", "Februar", "März", "April", "Mai", "Juni", "Juli", "August", "September", "Oktober", "November", "Dezember"};
        //Get Todays Date
        DateTime todayDate = DateTime.Today;
        int userId = args.ContainsKey("targetUserId") ? Convert.ToInt32(args["targetUserId"]) : Convert.ToInt32(args["userId"]);
        string userName = args.ContainsKey("targetUser") ? args["targetUser"].ToString() : args["user"].ToString();
		bool noInput = true;
		if(args.ContainsKey("targetUser")){
			noInput = false;
		}
        //Get the string of json
        string jsonIn = CPH.GetGlobalVar<string>("pwnBdaySystem", true);
        //Json Convert the jsonIn into the Hashtable
        Hashtable checkTable = JsonConvert.DeserializeObject<Hashtable>(jsonIn);
        //Need to get users as Json Array and then convert into the custom object
        JArray users = (JArray)checkTable["users"];
        List<BdayUser> bdayList = users.ToObject<List<BdayUser>>();
        
		//Check if user is in list
        BdayUser foundUser = bdayList.Find(userCheck => userCheck.UserId == userId);
        if (foundUser != null)
        {
            string userDateString = foundUser.Birthday.ToString("dd.MM");
            
            //init new DateTime
			DateTime userDate = new DateTime();
            
            
            bool leapYear = DateTime.IsLeapYear(todayDate.Year);
            bool leapBirth = false;

            //Check if it's a leap birthday
            if(foundUser.Birthday.Day == 29 && foundUser.Birthday.Month == 02)
            {
                leapBirth = true;
            }

            //Check if current Year is a Leap Year           
            if(!leapYear && leapBirth)
            {
                userDate = new DateTime(todayDate.Year, foundUser.Birthday.Month, 28);

            }else{

                userDate = new DateTime(todayDate.Year, foundUser.Birthday.Month, foundUser.Birthday.Day);
            }

            TimeSpan untilDate = userDate - todayDate;
            int untilDateDays = untilDate.Days;

            if(untilDateDays < 0)
            {
                leapYear = DateTime.IsLeapYear(todayDate.Year + 1);
                if(!leapYear && leapBirth){
                    userDate = new DateTime(todayDate.Year + 1, userDate.Month, 28);
                }else{
					
					userDate = new DateTime(todayDate.Year + 1, userDate.Month, userDate.Day);
                }
                
                untilDate = userDate - todayDate ;
                untilDateDays = untilDate.Days;

                if(!leapYear && leapBirth){
                    CPH.SendMessage($"{userName}({userDateString}) hat eigentlich kein Geburtstag nächstes Jahr... Egal, in ungefähr {untilDateDays} Tagen ist es soweit!");
                }else{
                    CPH.SendMessage($"{userName}({userDateString}) hat in {untilDateDays} Tagen Geburtstag!");
                }

            }else if (untilDateDays > 0)
            {
                if(!leapYear && leapBirth)
                {
                    CPH.SendMessage($"{userName}({userDateString}) hat in {untilDateDays} bzw. {untilDateDays +1 } Tagen Geburtstag! Schaltjahrkind gesichtet!<3");
                }else{
                    CPH.SendMessage($"{userName}({userDateString}) hat in {untilDateDays} Tagen Geburtstag!");
                }

            }else{
                if(!leapYear && leapBirth)
                {
                    CPH.SendMessage($"{userName}({userDateString}) hat ausnahmsweise heute Geburtstag! Alles Gute!");
                }else{
                    CPH.SendMessage($"{userName}({userDateString}) hat heute Geburtstag! Alles Gute!");
                }
            }
        }
        else
        {
			if(noInput){
				CPH.SendMessage("Trage deinen Geburtstag mit !bdayset DD.MM ein! Beispiel: !bdayset 30.06");
			}else{
				CPH.SendMessage("Kein Geburtstagseintrag gefunden für " + userName);
			}
            
        }

        return true;
    }


    [Serializable]
    public class BdayUser
    {
        [JsonProperty("userId", NullValueHandling = NullValueHandling.Ignore)]
        public int UserId { get; set; }

        [JsonProperty("userName", NullValueHandling = NullValueHandling.Ignore)]
        public string Username { get; set; }

        [JsonProperty("birthday", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Birthday { get; set; }

        [JsonProperty("discorded", NullValueHandling = NullValueHandling.Ignore)]
        public bool Discorded { get; set; }

        public override string ToString()
        {
            return $"UserId: {UserId}, Username: {Username}, Birthday: {Birthday.ToString("dd.MM.yyyy")}, Discorded: {Discorded}";
        }
    }
}