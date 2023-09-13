using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace JackLib
{
    public static class HttpClientExtension
    {
        public static async Task<T> GetAnsyc<T>(this HttpClient httpClient, string url, Dictionary<string, string> param)
        {
            try
            {
                StringBuilder apiUrl = new StringBuilder(url);
                if (param.Count > 0) apiUrl.Append('?');
                foreach (var p in param) apiUrl.Append($"{p.Key}={p.Value}&");
                apiUrl.Remove(apiUrl.Length - 1, 1);

                HttpResponseMessage response = await httpClient.GetAsync(apiUrl.ToString());

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(jsonResponse);
                    return jsonResponse.Parse<T>();
                }
                else
                {
                    Console.WriteLine($"HTTP 請求失敗，狀態碼：{response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"發生錯誤：{ex.Message}");
            }

            return default;
        }

        /**
        public async Task<string> PostAsync(string route, object param)
        {
            try
            {
                // Define the URL you want to send the POST request to
                string apiUrl = $"{_url}/{route}";

                // Serialize the data to JSON (you can use a library like Newtonsoft.Json)
                string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(param);

                // Create a StringContent object with the JSON data
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                // Send the POST request
                HttpResponseMessage response = await _client.PostAsync(apiUrl, content);

                // Check if the response is successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
                else
                {
                    Console.WriteLine("HTTP POST request failed with status code: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"發生錯誤：{ex.Message}");
            }

            return string.Empty;
        }
        **/
    }
}
