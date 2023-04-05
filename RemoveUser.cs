//Remove user from system
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class CPHInline
{
    public bool Execute()
    {
        int userId = int.Parse(args["userId"].ToString());
        string userName = args["user"].ToString();

		int setCounter = Convert.ToInt32(args["setCounter"]);


        string bdayJsonIn = CPH.GetGlobalVar<string>("pwnBdaySystem", true);
        Hashtable bdaySystem = JsonConvert.DeserializeObject<Hashtable>(bdayJsonIn);
        JArray users = (JArray)bdaySystem["users"];
        List<BdayUser> bdayList = users.ToObject<List<BdayUser>>();
        bool containsUser = bdayList.Exists(userCheck => userCheck.UserId == userId);
        if (!containsUser)
        {
            CPH.SendMessage("Nutzer hat noch kein Geburtstag gesetzt.");
        }
        else
        {
			
			int countLeft = 2 - setCounter;
			setCounter++;
            bdayList.RemoveAll(userCheck => userCheck.UserId == userId);
            bdaySystem["users"] = bdayList;
            string bdayJsonOut = JsonConvert.SerializeObject(bdaySystem);
            //Set global value to the string of json
            CPH.SetGlobalVar("pwnBdaySystem", bdayJsonOut, true);
			if(countLeft == 0){
				CPH.SendMessage($"Geburtstag von {userName} wurde entfernt. Du kannst kein neues Datum eintragen. Vielleicht ist ein Mod gnädig maluxsRage");
			}else{
				CPH.SendMessage($"Geburtstag von {userName} wurde entfernt. Du kannst nur noch einmal dein Datum neu eintragen.");
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