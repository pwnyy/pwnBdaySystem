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

		DateTime lastChecked = new DateTime(2000, today.Month, today.Day);

		CPH.LogInfo(lastChecked.ToString("dd.MM.yyyy"));
		bdaySystem.Add("lastChecked",lastChecked);
		bdaySystem.Add("currentYear",today.Year);
		
		List<BdayUser> userList = new List<BdayUser>();
		
		bdaySystem.Add("users",userList);

		string json = JsonConvert.SerializeObject(bdaySystem);
		CPH.SetGlobalVar("pwnBdaySystem",json,true);

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
