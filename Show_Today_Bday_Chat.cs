//Announce Birthday Users on Discord
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class CPHInline
{
    public bool Execute()
    {
        //Init Arrays
        List<string> todayCheck = new List<string>();
        //Init date from "today"
        DateTime dateToday = new DateTime(2000, DateTime.Today.Month, DateTime.Today.Day);
		//Check if LeapYear this year
		bool leapYear = DateTime.IsLeapYear(DateTime.Today.Year);

        //Get the string of json
        string jsonIn = CPH.GetGlobalVar<string>("pwnBdaySystem", true);
        //Json Convert the jsonIn into the Hashtable
        Hashtable bdaySystem = JsonConvert.DeserializeObject<Hashtable>(jsonIn);
        //Need to get users as Json Array and then convert into the custom object
        JArray users = (JArray)bdaySystem["users"];
        List<BdayUser> checkList = users.ToObject<List<BdayUser>>();
        //For each user in the BdayUser list give out information
        foreach (BdayUser user in checkList)
        {
            if (!user.Discorded)
            {	
				//Check timespan incase of leapyear
				TimeSpan checkUntil = dateToday - user.Birthday;
				//if it's not a leap year but the birthday of use is 29th February still add them to list
				if(!leapYear && user.Birthday.Day == 29 && user.Birthday.Month == 02){
					//Checks if today is 28th Feb or 1st March to add the leap year user to the todayList
					if(checkUntil.Days == -1 || checkUntil.Days == 1)
					{
						todayCheck.Add(user.Username);
						
					}
				//all other birthdays are getting checked normally
				}else if (DateTime.Compare(dateToday, user.Birthday) == 0)
                {
                    todayCheck.Add(user.Username);
					
                }
            }
			//Debugging
            //CPH.LogInfo(("UserId: " + user.UserId + " Username: " + user.Username + " Birthday: " + user.Birthday + " Discorded: " + user.Discorded));
        }
		
        
        string usersToday = "";

		int checkCount = 0;
        foreach (string name in todayCheck)
        {
            checkCount++;
            if (checkCount > 1)
            {
                if (checkCount == todayCheck.Count)
                {
                    usersToday = $"{usersToday} und {name}";
                }
                else if (checkCount < todayCheck.Count)
                {
                    usersToday = $"{usersToday}, {name}";
                }
            }
            else
            {
                usersToday = $"{name}";
            }
        }
		
        string verbPresent = todayCheck.Count > 1 ? "haben" : "hat";
        string discordMessage = "null";
        if (todayCheck.Count > 0)
        {
            CPH.SendMessage($"{usersToday} {verbPresent} heute Geburtstag! Alles Gute! maluxsLove");
        }
        else
        {
            CPH.SendMessage("Heute hat keiner Geburtstag ... maluxsCry");
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