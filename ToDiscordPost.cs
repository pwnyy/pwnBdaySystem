using System;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

public class CPHInline
{
    public string webhookUrl, contentJson, messageId;
    public bool Execute()
    {
        //Get Webhook Link pfrom argument
        webhookUrl = args["discordHook"].ToString();
        string embedTitle = args.ContainsKey("embedTitle") ? args["embedTitle"].ToString() : "Birthday Announcement";
        //Get HEX color value and convert to Decimal for Discord
        string hexColor = args.ContainsKey("embedColor.html") ? args["embedColor.html"].ToString() : "#000000";
        //Get Birthday Message
        string bdayMessage = args["discordMessage"].ToString();

        //Post To Discord
        PostDiscord(bdayMessage, embedTitle, hexColor, webhookUrl);

        return true;
    }
    public bool PostDiscord(string message,string embedTitle ,string hexColor,string webhookUrl)
    {
        //Convert hexColor to Decimal
        hexColor = hexColor.Substring(1, hexColor.Length - 1);
        int color = Convert.ToInt32(hexColor, 16);

        //Make JsonObject
        DiscordMessage discordObject = new DiscordMessage();
        //Set normal content to nothing
        discordObject.Content = "";
        //Create Embed Object
        Embed embedInfo = new Embed()
        {Title = embedTitle, Description = message, Color = color};
        //Create a list of Embed for DiscordMessage Object
        List<Embed> embedList = new List<Embed>(1);
        //Adding the Embed Object to a Embed List
        embedList.Add(embedInfo);
        //Set Embed List into DiscordMessage
        discordObject.Embeds = embedList;
        //Serialize the DiscordMessage Object into JSON to send per HTTP Request
        string json = JsonConvert.SerializeObject(discordObject);

        //Send To Discord
        using (var client = new HttpClient())
        {
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(new HttpMethod("POST"), webhookUrl)
            {Content = httpContent};
            var result = client.SendAsync(request).Result;
        }
        return true;
        
    }
    

    public partial class DiscordMessage
    {
        [JsonProperty("content", NullValueHandling = NullValueHandling.Ignore)]
        public string Content { get; set; }

        [JsonProperty("embeds", NullValueHandling = NullValueHandling.Ignore)]
        public List<Embed> Embeds { get; set; }
    }

    public partial class Embed
    {
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public long? Color { get; set; }

    }

}