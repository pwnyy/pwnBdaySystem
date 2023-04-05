//Add User to System
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

public class CPHInline
{
    public bool Execute()
    {
		//Month List
		string[] monthList = {"Januar","Februar","März","April","Mai","Juni","Juli","August","September","Oktober","November","Dezember"}; 
        
		//Get User Info
		int userId = int.Parse(args["userId"].ToString());
        string userName = args["user"].ToString();

		//Get Set Counter
		int setCounter = Convert.ToInt32(args["setCounter"]);
        
		//Get Input
        string inputDate = !(args["rawInput"].ToString() == "") ? args["rawInput"].ToString() : "null";
        //Set Date regex pattern
        string pattern = @"^(3[01]|[12][0-9]|0[1-9]).(1[0-2]|0[1-9])$";
        //Check if input is valid date
        bool isDate = Regex.IsMatch(inputDate, pattern);
        
		if(setCounter == 2){
			CPH.SendMessage("Dein Limit deinen Geburtstag neu einzutragen wurde erreicht. Vielleicht gibt ein Mod dir ja noch eine Chance maluxsRage");
			return false;
		}
		
		//If input matches regex pattern
		if (isDate)
        {
            //Check if the date is possible or not if not isDate = false
            switch (inputDate)
            {
                case "30.02":
                case "31.02":
                case "31.04":
                case "31.06":
                case "31.09":
                case "31.11":
                    isDate = false;
                    break;
            }
        }
		//Check if input is empty
        if (inputDate == "null")
        {
            CPH.SendMessage("Eingabe soll TT.MM sein. Beispiel: 30.06");
        }
		//Check if input is not valid
        else if (!isDate)
        {
            CPH.SendMessage("Kein gültiges Datum oder Format. Beispiel: 30.06");
        }
        else
        {
			//Split Input in two days and months
            string[] dateParts = inputDate.Split('.');
			//Set ints for dateDay,dateMonth
			int dateDay = int.Parse(dateParts[0]);
            int dateMonth = int.Parse(dateParts[1]);
			//Get Bdaysystem
            string bdayJsonIn = CPH.GetGlobalVar<string>("pwnBdaySystem", true);
            //Convert json to Hashtable
			Hashtable bdaySystem = JsonConvert.DeserializeObject<Hashtable>(bdayJsonIn);
            //Get user list
			JArray users = (JArray)bdaySystem["users"];
            //Convert to List of BdayUser
			List<BdayUser> bdayList = users.ToObject<List<BdayUser>>();
            
			//CPH.SendMessage(userId.ToString());
			//Check if user is already in the list
            bool containsUser = bdayList.Exists(userCheck => userCheck.UserId == userId);
            if (containsUser)
            {
                CPH.SendMessage("Geburtstag für " + userName + " wurde schon eingetragen.");
            }
			//If not in list already
			else
            {
				setCounter++;
				//Set UserVar Counter for setting a birthday
				CPH.SetUserVar(userName,"pwnBdays_Set",setCounter,true);
			
				//Create Bday DateTime
                BdayUser newBday = new BdayUser{UserId = userId, Username = userName, Birthday = new DateTime(2000, dateMonth, dateDay), Discorded = false};
                //Add User to the list
				bdayList.Add(newBday);
				//Set List to the Hashtable
                bdaySystem["users"] = bdayList;
				//Convert Hashtable to Json to save to database as string
                string bdayJsonOut = JsonConvert.SerializeObject(bdaySystem);
                //Set global value to the string of json
                CPH.SetGlobalVar("pwnBdaySystem", bdayJsonOut, true);
				CPH.LogInfo($"[pwn Bday] - User {userName} with user id {userId} set their birthday to {dateDay}.{dateMonth}/{monthList[dateMonth-1]}.");
                CPH.SendMessage(dateDay +". "+ monthList[dateMonth -1] + " wurde für " + userName + " als Geburtstag eingetragen.");
            }
        }

        return true;
    }
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