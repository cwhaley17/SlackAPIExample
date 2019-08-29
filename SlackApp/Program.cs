using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SlackExample
{
    class SendMessageExample
    {
        private static readonly HttpClient client = new HttpClient();
        public class SlackMessageResponse
        {
            public bool ok { get; set; }
            public string error { get; set; }
            public string channel { get; set; }
            public string ts { get; set; }
        }
        public class SlackMessage
        {
            public string channel { get; set; }
            public string channel_id { get; set; }
            public string configuration_url { get; set; }
            public string url { get; set; }
            public string text { get; set; }
            public bool as_user { get; set; }
            public SlackAttachment[] attachments { get; set; }
        }
        public class SlackAttachment
        {
            public string fallback { get; set; }
            public string text { get; set; }
            public string image_url { get; set; }
            public string color { get; set; }
        }

        // sends a slack message asynchronous
        // throws exception if message can not be sent
        public static async Task SendMessageAsync(string token, SlackMessage msg)
        {
            // serialize method parameters to JSON
            var content = JsonConvert.SerializeObject(msg);
            var httpContent = new StringContent(
                content,
                Encoding.UTF8,
                "application/json"
            );

            // set token in authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.PostAsync("x", httpContent); 

            // fetch response from API
            string responseJson = await response.Content.ReadAsStringAsync();

            // convert JSON response to object
            //SlackMessageResponse messageResponse =
               // JsonConvert.DeserializeObject<SlackMessageResponse>(responseJson);

            if (responseJson.ToLower() != "ok")
            {
                throw new Exception(
                    "failed to send message. error: " + responseJson
                );
            }
        }

        static void Main(string[] args)
        {

            SlackClient slack = new SlackClient("https://slack.com/oauth/authorize");
            slack.PostMessage("", "", "");

            var msg = new SlackMessage
            {
                channel = "testroom",
                text = "New App !!!!!! ",
                as_user = true,
                attachments = new SlackAttachment[]
                {
                    new SlackAttachment
                    {
                        fallback = "this did not work",
                        text ="You are now a test subject for the Slack integration app. Welcome to the dark side! :ghost:",
                        color = "good"
                    }
                }
            };

            SendMessageAsync(
                "xoxp-xx",
                msg
            ).Wait();

            Console.WriteLine("Message has been sent");
            Console.ReadKey();

        }

        // probably not even needed? https://api.slack.com/docs/oauth#
        private enum SlackError
        {
            invalid_payload = 0,
            user_not_found,
            channel_not_found,
            channel_is_archived,
            action_prohibited,
            posting_to_general_channel_denied,
            too_many_attachments,
            no_service,
            no_service_id,
            no_team,
            team_disabled,
            invalid_token
        }
    }
}