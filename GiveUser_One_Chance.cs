using System;

public class CPHInline
{
	public bool Execute()
	{
		string userName = args.ContainsKey("targetUser") ? args["targetUser"].ToString() :"null";

		if(userName == "null")
		{
			CPH.SendMessage("Name des Benutzers wird gebraucht.");
		}else{
			CPH.SetUserVar(userName,"pwnBdays_Set",1,true);
			CPH.SendMessage($"{userName}, dir wurde noch eine Chance gewährt.. Beispiel: !bdayset 30.06");
		}
		return true;
	}
}
