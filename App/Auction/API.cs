using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
                JObject answer = await request(values);
                return answer;
        }

        public static async Task<JObject> request(Dictionary<string, string> param) 
        {
            JObject json = new JObject();
            HttpClient httpClient = new HttpClient();
            Uri uri = new Uri(API_URL + "/customers");

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
       

    }
}
