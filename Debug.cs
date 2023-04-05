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
		string jsonIn = CPH.GetGlobalVar<string>("pwnBdaySystem",true);
		CPH.LogInfo(jsonIn);
		//Json Convert the jsonIn into the Hashtable
		Hashtable checkTable = JsonConvert.DeserializeObject<Hashtable>(jsonIn);
		//Need to get users as Json Array and then convert into the custom object
		JArray users = (JArray)checkTable["users"];
		List<BdayUser> checkList = users.ToObject<List<BdayUser>>();

		//For each user in the BdayUser list give out information
		foreach(BdayUser user in checkList)
		{
			CPH.LogInfo(("UserId: " + user.UserId + " Username: " + user.Username + " Birthday: " + user.Birthday + " Discorded: " + user.Discorded));
		}
		
		DateTime testDate = new DateTime(2000,12,08);
		int testCheck = checkList.FindLastIndex(x => x.Birthday == testDate);
		DateTime newCheckTime = new DateTime();
		
		if(testCheck == checkList.Count-1)
		{
			newCheckTime = checkList[0].Birthday;
		}else{
			newCheckTime = checkList[testCheck+1].Birthday;
		}
		
		CPH.LogInfo(newCheckTime.ToString());
		foreach(BdayUser user in checkList)
		{
			if(user.Birthday == newCheckTime){
			CPH.LogInfo(("UserId: " + user.UserId + " Username: " + user.Username + " Birthday: " + user.Birthday + " Discorded: " + user.Discorded));
			}
		}
		CPH.LogInfo(testCheck.ToString());
		//CPH.LogInfo(.ToString());

		
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
