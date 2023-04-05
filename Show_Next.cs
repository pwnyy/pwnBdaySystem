using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class CPHInline
{
    public bool Execute()
    {
        //Get the string of json
        string jsonIn = CPH.GetGlobalVar<string>("pwnBdaySystem", true);
        //Json Convert the jsonIn into the Hashtable
        Hashtable checkTable = JsonConvert.DeserializeObject<Hashtable>(jsonIn);
        //Need to get users as Json Array and then convert into the custom object
        JArray users = (JArray)checkTable["users"];
        List<BdayUser> bdayList = users.ToObject<List<BdayUser>>();
        
		//Get "Todays" Date
		DateTime checkDate = new DateTime(2000,DateTime.Today.Month,DateTime.Today.Day);
		DateTime nextBday = new DateTime();
		
		//Get count of all entries
		int listCount = bdayList.Count;
		//Set a count through how many entries it goes
		int loopCount = 0;
		foreach(BdayUser user in bdayList)
		{
			loopCount++;
			//if a date is later than the current date set that date as nextbday and break
			int compareDates = DateTime.Compare(user.Birthday,checkDate);
			if(compareDates > 0){
				
				nextBday = user.Birthday;

				break;
			}
			//if it reaches the last entry but still no nextbday then set first entry as nextbday as it's the next year
			if(loopCount == listCount)
			{
				nextBday = bdayList[0].Birthday;
			}
		}

		//List of users that match nextbday
		List<string> nextBdayList = new List<string>();

		//In case there are only entries of the same date
		if(nextBday == checkDate){
			CPH.SendMessage("Sonst hat keiner dieses Jahr Geburtstag...");
			return false;
		}else{
			//Get list of usernames
			foreach(BdayUser user in bdayList)
			{
				if(user.Birthday == nextBday)
				{
					nextBdayList.Add(user.Username);
				}
			}
		}

		//Check if current year is a leap year and check if nextBday is a leap year birthday
		bool leapYear = DateTime.IsLeapYear(DateTime.Today.Year);
		bool leapBirth = false;
		if(nextBday.Month == 02 && nextBday.Day == 29){ leapBirth = true;}
		
		//if no leap year but leap birth then set date to 28 to make it valid
		if(!leapYear && leapBirth){
			nextBday = new DateTime(DateTime.Today.Year,02,28);
		}

		//Set nextBday to current year
		nextBday = new DateTime(DateTime.Today.Year,nextBday.Month,nextBday.Day);

		//incase previously the loopcount reached the end of the entries it is knows the next birthday will be in the next year
		if(loopCount == listCount){
			//Add a year to nextBday
			nextBday.AddYears(1);
			
			//also check if leap year and if it's leap birth and adjust accordingly
			leapYear = DateTime.IsLeapYear(nextBday.Year +1);
			if(!leapYear && leapBirth)
			{
				nextBday = new DateTime(nextBday.Year,02,28);
			}
		}
		
		//Check how many days until next birthday
		TimeSpan untilDate = nextBday - DateTime.Today;
        int untilDateDays = untilDate.Days;
		
		//string for the users in message
        string usersNext = "";

		int checkCount = 0;
        foreach (string name in nextBdayList)
        {
            checkCount++;
            if (checkCount > 1)
            {
                if (checkCount == nextBdayList.Count)
                {
                    usersNext = $"{usersNext} und {name}";
                }
                else if (checkCount < nextBdayList.Count)
                {
                    usersNext = $"{usersNext}, {name}";
                }
            }
            else
            {
                usersNext = $"{name}";
            }
        }
		
		string[] monthList = {"Januar", "Februar", "März", "April", "Mai", "Juni", "Juli", "August", "September", "Oktober", "November", "Dezember"};

        string verbPresent = nextBdayList.Count > 1 ? "haben" : "hat";
        string discordMessage = "null";
        if (nextBdayList.Count > 0)
        {
			//In case of not being a leap year but it being a leapbirth
			if(!leapYear && leapBirth)
			{
				CPH.SendMessage($"Als nächstes {verbPresent} {usersNext} in {untilDateDays} Tagen am {nextBday.Day}. {monthList[nextBday.Month - 1]} Geburtstag! Naja eigentlich am 29. aber wir sind ja nicht so pingelig! maluxsLove");
			}else{
				CPH.SendMessage($"Als nächstes {verbPresent} {usersNext} in {untilDateDays} Tagen am {nextBday.Day}. {monthList[nextBday.Month - 1]} Geburtstag! maluxsLove");
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