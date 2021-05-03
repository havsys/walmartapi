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
            string url = URL_PREFIX + "product/v2/stores?zip=" + zipCode;

            addHeaders(httpClient);
            try
            {
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                var responseContent = response.Content;
                departmentList = responseContent.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                resetAuthSignature();
            }
            return departmentList;
        }

        public string getTaxonomyList()
        {
            string taxonomyList = "";
            string url = URL_PREFIX + "product/v2/taxonomy";

            addHeaders(httpClient);
            try
            {
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                var responseContent = response.Content;
                taxonomyList = responseContent.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                resetAuthSignature();
            }
            return taxonomyList;
        }

        public string search(string categoryId, string query, int numberOfItems, int startOffSet)
        {
            string searchResult = "";
            if (query == "")
                return searchResult;

            if (numberOfItems < 0) numberOfItems = 10;
            if (numberOfItems > 25) numberOfItems = 25;

            if (startOffSet < 0) startOffSet = 0;

            string url = URL_PREFIX + "v2/search?query=" + query + "&numItems=" + numberOfItems.ToString() + "&start=" + startOffSet.ToString() +  ((categoryId != "") ? "&categoryId=" + categoryId : "");

            Console.WriteLine(url);

            addHeaders(httpClient);
            try
            {
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                var responseContent = response.Content;
                searchResult = responseContent.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                resetAuthSignature();
            }
            return searchResult;
        }

        private void resetAuthSignature()
        {
            authSignature = "";
        }

        private void addHeaders(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Add(SignatureGenerator.PRIVATEKEYVERSIONKEY, SignatureGenerator.PRIVATE_KEY_VERSION);
            httpClient.DefaultRequestHeaders.Add(SignatureGenerator.CONSUMERIDKEY, SignatureGenerator.CONSUMER_ID_PROD);
            string timeNow = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
            Console.WriteLine(timeNow);
            httpClient.DefaultRequestHeaders.Add(SignatureGenerator.CONSUMERINTIMESTAMPKEY, timeNow);
            if (authSignature == "")
            {
                authSignature = SignatureGenerator.generateSignature(timeNow);
                Console.WriteLine(authSignature);
            }
            httpClient.DefaultRequestHeaders.Add(SignatureGenerator.AUTHSIGNATUREKEY, authSignature);
        }
    }
}
