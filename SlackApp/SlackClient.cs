using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;

//A simple C# class to post messages to a Slack channel  
//Note: This class uses the Newtonsoft Json.NET serializer available via NuGet  
public class SlackClient
{
    private readonly Uri _uri;
    private readonly Encoding _encoding = new UTF8Encoding();

    public SlackClient(string urlWithAccessToken)
    {
        _uri = new Uri(urlWithAccessToken);
    }

    //Post a message using simple strings  
    public void PostMessage(string text, string username = null, string channel = null)
    {
        Payload payload = new Payload()
        {
            ClientId = "",
            ClientSecret = "",
            Code = "",
            RedirectUrl = "",
            Scope = ""
        };

        PostMessage(payload);
    }

    //Post a message using a Payload object  
    public void PostMessage(Payload payload)
    {
        try
        {
            string payloadJson = JsonConvert.SerializeObject(payload);

            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                NameValueCollection data = new NameValueCollection();
                data["payload"] = payloadJson;
                

                var response = client.UploadValues(_uri, "POST", data);

                //The response text is usually "ok"  
                string responseText = _encoding.GetString(response);
                var holder = responseText;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

public class Payload
{
    [JsonProperty("client_id")]
    public string ClientId { get; set; }

    [JsonProperty("client_secret")]
    public string ClientSecret { get; set; }

    [JsonProperty("code")]
    public string Code { get; set; }

    [JsonProperty("redirect_uri")]
    public string RedirectUrl { get; set; }

    [JsonProperty("single_channel")]
    public bool SingleChannel { get; set; }

    [JsonProperty("scope")]
    public string Scope { get; set; }
}