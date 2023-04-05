//Get Bday Users
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class CPHInline
{
	public bool Execute()
	{
		Hashtable bdaySystem = new Hashtable();
		DateTime today = DateTime.Today;

		DateTime lastChecked = new DateTime(2001, today.Month, today.Day);
		
		CPH.LogInfo(lastChecked.ToString("dd.MM.yyyy"));
		bdaySystem.Add("lastChecked",lastChecked);
		bdaySystem.Add("currentYear",today.Year);
		
		List<BdayUser> userList = new List<BdayUser>();
		
		BdayUser newUser1 = new BdayUser();
		newUser1.UserId = 123456;
		newUser1.Username = "test";
		DateTime newBday = new DateTime(2001, today.Month, today.Day);
		newUser1.Birthday = newBday;
		newUser1.Discorded = false;
		
		userList.Add(newUser1);

		BdayUser newUser2 = new BdayUser();
		newUser2.UserId = 987654321;
		newUser2.Username = "test2";
		//DateTime newBday = new DateTime(2001, today.Month, today.Day);
		newUser2.Birthday = newBday;
		newUser2.Discorded = false;
		
		userList.Add(newUser2);
		
		bdaySystem.Add("users",userList);
		CPH.LogInfo("test1");
		//Serialize into normal json using newtonsoft.json
		string json = JsonConvert.SerializeObject(bdaySystem);
		//Set global value to the string of json
		CPH.SetGlobalVar("pwnBdaySystem",json,true);
		CPH.LogInfo("test2");
		
		//Get the string of json
		string jsonIn = CPH.GetGlobalVar<string>("pwnBdaySystem",true);
		CPH.LogInfo(jsonIn);
		CPH.LogInfo("test3");
		//Json Convert the jsonIn into the Hashtable
		Hashtable checkTable = JsonConvert.DeserializeObject<Hashtable>(jsonIn);
		CPH.LogInfo("test4");
		//Need to get users as Json Array and then convert into the custom object
		JArray users = (JArray)checkTable["users"];
		List<BdayUser> checkList = users.ToObject<List<BdayUser>>();

		CPH.LogInfo("test5");
		//For each user in the BdayUser list give out information
		foreach(BdayUser user in checkList)
		{
			CPH.LogInfo(("UserId: " + user.UserId + " Username: " + user.Username + " Birthday: " + user.Birthday + " Discorded: " + user.Discorded));
		}
		
		return true;
	}


	[Serializable]
	public class BdayUser{
		[JsonProperty("userId", NullValueHandling = NullValueHandling.Ignore)]
		public int UserId {get;set;}
		[JsonProperty("userName", NullValueHandling = NullValueHandling.Ignore)]
		public string Username {get;set;}
		[JsonProperty("birthday", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime Birthday {get;set;}
		[JsonProperty("discorded", NullValueHandling = NullValueHandling.Ignore)]
		public bool Discorded {get;set;}

		public override string ToString()
		{
			return $"UserId: {UserId}, Username: {Username}, Birthday: {Birthday.ToString("dd.MM.yyyy")}, Discorded: {Discorded}";
		}
	}

}
