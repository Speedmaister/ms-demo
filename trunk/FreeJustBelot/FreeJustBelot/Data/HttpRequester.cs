using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using FreeJustBelot.Models;

namespace FreeJustBelot.Data
{
    public class HttpRequester
    {
        private string baseUrl;
        private HttpClient client;

        public HttpRequester(string baseUrl)
        {
            this.baseUrl = baseUrl;
            this.client = new HttpClient();
        }

        public T Get<T>(string serviceUrl, string mediaType = "application/json")
        {
            var request = new HttpRequestMessage();

            request.RequestUri = new Uri(this.baseUrl + serviceUrl);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            request.Method = HttpMethod.Get;

            var response = client.SendAsync(request).Result;

            var returnObj = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(returnObj);
        }

        public T Post<T>(string serviceUrl, object data, string mediaType = "application/json")
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(this.baseUrl + serviceUrl);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            request.Method = HttpMethod.Post;

            request.Content = new StringContent(JsonConvert.SerializeObject(data));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            var response = client.SendAsync(request).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            try
            {
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (JsonReaderException ex)
            {
                var error = JsonConvert.DeserializeObject<MessageModel>(content);
                throw new InvalidOperationException(error.Message);
            }
        }

        public T Put<T>(string serviceUrl, object data, string mediaType = "application/json")
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(this.baseUrl + serviceUrl);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            request.Method = HttpMethod.Put;

            request.Content = new StringContent(JsonConvert.SerializeObject(data));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            var response = client.SendAsync(request).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            try
            {
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (JsonReaderException ex)
            {
                var error = JsonConvert.DeserializeObject<MessageModel>(content);
                throw new InvalidOperationException(error.Message);
            }
        }

        public Task<T> CreateGetRequestAsync<T>(string serviceUrl, string mediaType = "application/json")
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(this.baseUrl + serviceUrl);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            request.Method = HttpMethod.Get;

            return client.SendAsync(request).ContinueWith(
                (task) =>
                {
                    var response = task.Result;
                    var content = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<T>(content);
                    return result;
                });
        }

        public Task<T> PostAsync<T>(string serviceUrl,
            object data, string mediaType = "application/json")
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(this.baseUrl + serviceUrl);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            request.Method = HttpMethod.Post;

            request.Content = new StringContent(JsonConvert.SerializeObject(data));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            return client.SendAsync(request).ContinueWith((task) =>
            {
                var response = task.Result;
                var content = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<T>(content);
                return result;
            });
        }

        public Task<HttpResponseMessage> PostAsync(string serviceUrl,
            object data, string mediaType = "application/json")
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(this.baseUrl + serviceUrl);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            request.Method = HttpMethod.Post;

            request.Content = new StringContent(JsonConvert.SerializeObject(data));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            var response = client.SendAsync(request);
            return response;
        }

        public Task<T> PutAsync<T>(string serviceUrl,
            object data, string mediaType = "application/json")
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(this.baseUrl + serviceUrl);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            request.Method = HttpMethod.Put;

            request.Content = new StringContent(JsonConvert.SerializeObject(data));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            return client.SendAsync(request).ContinueWith((task) =>
            {
                var response = task.Result;
                var content = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<T>(content);
                return result;
            });
        }
    }
}
