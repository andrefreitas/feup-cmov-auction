using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Auction
{
    class API
    {
        const string API_URL = "http://neo.andrefreitas.pt:8083/api";

        public static async Task<JObject> register(String name, String email, String password)
        {
            var values = new Dictionary<string, string>();
            values.Add("name", name);
            values.Add("email", email);
            values.Add("password", password);
            JObject answer = await postRequest(values, "/customers");
            return answer;
        }

        public static async Task<JObject> login(String email, String password)
        {
            var values = new Dictionary<string, string>();
            values.Add("email", email);
            values.Add("password", password);
            JObject answer = await postRequest(values, "/login");
            return answer;
        }

        public static async Task<JArray> getAuctions()
        {
            var response = await getRequest("/auctions");
            var responseString = await response.Content.ReadAsStringAsync();
            JArray json = JArray.Parse(responseString);
            return json;
        }

        public static async Task<JObject> subscribe(string auctionID, string customerID, string channelURI)
        {
            var values = new Dictionary<string, string>();
            values.Add("customerID", customerID);
            values.Add("auctionID", auctionID);
            values.Add("channelURI", channelURI);
            JObject answer = await postRequest(values, "/subscribe");
            return answer;
        }

        public static async Task<JObject> postRequest(Dictionary<string, string> param, String path)
        {
            JObject json = new JObject();
            HttpClient httpClient = new HttpClient();
            Uri uri = new Uri(API_URL + path);

            httpClient.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
            var content = new FormUrlEncodedContent(param);
            var response = await httpClient.PostAsync(uri, content);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new Exception("404");
            }
            var responseString = await response.Content.ReadAsStringAsync();
            json = JObject.Parse(responseString);
            return json;
        }

        public static async Task<HttpResponseMessage> getRequest(String path)
        {
            JObject json = new JObject();
            HttpClient httpClient = new HttpClient();
            Uri uri = new Uri(API_URL + path);

            httpClient.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
            var response = await httpClient.GetAsync(uri);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new Exception("404");
            }
            return response;
        }
    }
}
