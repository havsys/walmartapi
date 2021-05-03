using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WalmartApiClient
{
    class WalmartApiClient
    {
        public static string URL_PREFIX = "https://developer.api.walmart.com/api-proxy/service/affil/";
        
        private HttpClient httpClient;
        private string authSignature = "";
        

        public WalmartApiClient()
        {
            httpClient = new HttpClient();
        }

        public string getDepartmentList(string zipCode)
        {
            string departmentList = "";
            string url = URL_PREFIX + "/product/v2/stores?zip=" + zipCode;

            addHeaders(httpClient);
            try
            {
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                var responseContent = response.Content;
                departmentList = responseContent.ReadAsStringAsync().Result;
                return departmentList;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                resetAuthSignature();
                return "";
            }
        }

        private void resetAuthSignature()
        {
            authSignature = "";
        }

        private void addHeaders(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Add(SignatureGenerator.PRIVATEKEYVERSIONKEY, SignatureGenerator.PRIVATE_KEY_VERSION);
            httpClient.DefaultRequestHeaders.Add(SignatureGenerator.CONSUMERIDKEY, SignatureGenerator.CONSUMER_ID_PROD);
            string timeNow = (DateTimeOffset.Now.ToUnixTimeMilliseconds() - 2000000).ToString();
            Console.WriteLine(timeNow);
            httpClient.DefaultRequestHeaders.Add(SignatureGenerator.CONSUMERINTIMESTAMPKEY, timeNow);
            if (authSignature == "")
            {
                authSignature = SignatureGenerator.generateSignature(timeNow);
            }
            httpClient.DefaultRequestHeaders.Add(SignatureGenerator.AUTHSIGNATUREKEY, authSignature);
        }
    }
}
