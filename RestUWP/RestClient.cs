using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Imagine.Uwp.Rest
{
    public enum HttpScheme
    {
        Http,
        Https
    }

    public class RestClient
    {
        /// <summary>
        /// http or https
        /// </summary>
        public String Scheme { get; set; }

        /// <summary>
        /// http content
        /// </summary>
        public String Content { get; set; }

        public Uri Uri { get; set; }

        /// <summary>
        /// Host: yourhost.com
        /// </summary>
        public String Host { get; set; } = String.Empty;

        /// <summary>
        /// Path: /api/user/yourpath
        /// </summary>
        public String Path { get; set; } = String.Empty;

        /// <summary>
        /// HttpMethod 
        /// </summary>
        public HttpMethod Method { get; set; } = HttpMethod.Get;

        public List<object> Queries { get; set; } = new List<object>();

        public Dictionary<String, String> Headers { get; set; } = new Dictionary<string, string>();

        public Dictionary<String, String> Contents { get; set; } = new Dictionary<string, string>();

        public RestClient(String host, String path, Dictionary<String, String> headers)
        {
            this.Host = host;
            this.Path = Path;
            this.Headers = headers;
        }


        public RestClient(String host, String path, Dictionary<String, String> headers, Dictionary<String, String> contents)
        {
            this.Host = host;
            this.Path = Path;
            this.Headers = headers;
            this.Contents = contents;
        }

        public RestClient(String host, String path)
        {
            this.Host = host;
            this.Path = Path;
        }

        public RestClient(Uri uri)
        {
            this.Uri = uri;
        }

        public RestClient(HttpScheme scheme, String host, String path)
        {
            this.Scheme = scheme.ToString();
            this.Host = host;
            this.Path = Path;
        }

        public RestClient(HttpScheme scheme, String host, String path, params object[] queries)
        {
            this.Scheme = scheme.ToString();
            this.Host = host;
            this.Path = Path;
            if (queries != null && queries.Any())
                this.Queries = queries.ToList();
        }

        public RestClient(HttpScheme scheme, String host, String path, HttpMethod method)
        {
            this.Scheme = scheme.ToString();
            this.Host = host;
            this.Path = Path;
            this.Method = method;
        }

        public RestClient(String host, String path, HttpMethod method)
        {
            this.Host = host;
            this.Path = Path;
            this.Method = method;
        }

        public RestClient(String host, HttpMethod method)
        {
            this.Host = host;
            this.Method = method;
        }

        public RestClient(String host)
        {
            this.Host = host;
        }

        public void SetContent(String content)
        {
            if (String.IsNullOrEmpty(content)) return;
            this.Content = content;
        }

        public RestClient()
        {

        }

        public void AddParameters(Dictionary<String, String> parameters)
        {
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    this.Contents[parameter.Key] = parameter.Value;
                }
            }

        }

        public void AddParameter(String key, String value)
        {
            Contents[key] = value;
        }

        public void AddHeader(String key, String value)
        {
            Headers[key] = value;
        }

        public void SetContents(Dictionary<String, String> data)
        {
            if (data != null)
                this.Contents = data;
        }

        public async Task<T> ExecuteAsync<T>(Action<HttpResponseMessage> callBack = null)
        {
            var result = default(T);
            if (String.IsNullOrEmpty(Host)) return result;
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage.Method = Method;
            ExtractHeaders(httpClient);
            var uriBuilder = this.Uri != null ? new UriBuilder(Uri) : new UriBuilder
            {
                Host = Host,
                Path = BuildPath()
            };
            if (!String.IsNullOrEmpty(Scheme))
                uriBuilder.Scheme = Scheme;
            String _requestResult = String.Empty;
            if (Contents != null && Contents.Any())
            {
                if (Method == HttpMethod.Get)
                {
                    string query = BuildQueryString();
                    uriBuilder.Query = query;
                }
                else if (Method == HttpMethod.Post)
                {
                    if (!String.IsNullOrEmpty(Content))
                    {
                        requestMessage.Content = new StringContent(Content);
                    }
                    else if (Contents != null && Contents.Any())
                    {
                        requestMessage.Content = new FormUrlEncodedContent(Contents);
                    }
                }
            }
            requestMessage.RequestUri = new Uri(uriBuilder.Uri.ToString(), UriKind.Absolute);

            try
            {
                HttpResponseMessage responseMessage = await httpClient.SendAsync(requestMessage).ConfigureAwait(false);

                if (callBack != null)
                {
                    callBack(responseMessage);
                }

                if (responseMessage != null)
                {
                    responseMessage.EnsureSuccessStatusCode();
                    String responseContent = await responseMessage.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(responseContent)) return default(T);
                    result = JsonConvert.DeserializeObject<T>(responseContent);
                }
            }
            catch
            {
                if (callBack != null)
                {
                    callBack(null);
                }
            }

            return result;
        }

        public async Task<String> RequestStringAsync(Action<HttpResponseMessage> callBack = null)
        {
            if (String.IsNullOrEmpty(Host)) return null;

            HttpClient httpClient = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage.Method = Method;
            ExtractHeaders(httpClient);
            var uriBuilder = this.Uri != null ? new UriBuilder(Uri) : new UriBuilder
            {
                Host = Host,
                Path = BuildPath()
            };
            if (!String.IsNullOrEmpty(Scheme))
                uriBuilder.Scheme = Scheme;
            String _requestResult = String.Empty;
            if (Contents != null && Contents.Any())
            {
                if (Method == HttpMethod.Get)
                {
                    string query = BuildQueryString();
                    uriBuilder.Query = query;
                }
                else if (Method == HttpMethod.Post)
                {
                    if (!String.IsNullOrEmpty(Content))
                    {
                        requestMessage.Content = new StringContent(Content);
                    }
                    else if (Contents != null && Contents.Any())
                    {
                        requestMessage.Content = new FormUrlEncodedContent(Contents);
                    }
                }
            }

            requestMessage.RequestUri = new Uri(uriBuilder.Uri.ToString(), UriKind.Absolute);

            try
            {
                HttpResponseMessage responseMessage = await httpClient.SendAsync(requestMessage).ConfigureAwait(false);

                if (callBack != null)
                {
                    callBack(responseMessage);
                }

                if (responseMessage != null)
                {
                    responseMessage.EnsureSuccessStatusCode();
                    return await responseMessage.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                if (callBack != null)
                {
                    callBack(null);
                }
            }

            return null;
        }

        public async Task<byte[]> RequestBytesAsync(Action<HttpResponseMessage> callBack = null)
        {
            if (String.IsNullOrEmpty(Host)) return new byte[0];

            HttpClient httpClient = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage.Method = Method;
            ExtractHeaders(httpClient);
            var uriBuilder = this.Uri != null ? new UriBuilder(Uri) : new UriBuilder
            {
                Host = Host,
                Path = BuildPath()
            };
            if (!String.IsNullOrEmpty(Scheme))
                uriBuilder.Scheme = Scheme;
            String _requestResult = String.Empty;
            if (Contents != null && Contents.Any())
            {
                if (Method == HttpMethod.Get)
                {
                    string query = BuildQueryString();
                    uriBuilder.Query = query;
                }
                else if (Method == HttpMethod.Post)
                {
                    if (!String.IsNullOrEmpty(Content))
                    {
                        requestMessage.Content = new StringContent(Content);
                    }
                    else if(Contents != null && Contents.Any())
                    {
                        requestMessage.Content = new FormUrlEncodedContent(Contents);
                    }
                }
            }
            requestMessage.RequestUri = new Uri(uriBuilder.Uri.ToString(), UriKind.Absolute);

            try
            {
                HttpResponseMessage responseMessage = await httpClient.SendAsync(requestMessage).ConfigureAwait(false);

                if (callBack != null)
                {
                    callBack(responseMessage);
                }

                if (responseMessage != null)
                {
                    responseMessage.EnsureSuccessStatusCode();
                    return await responseMessage.Content.ReadAsByteArrayAsync();
                }
            }
            catch
            {
                if (callBack != null)
                {
                    callBack(null);
                }
            }

            return null;
        }

        public async Task<Stream> RequestStreamAsync(Action<HttpResponseMessage> callBack = null)
        {
            if (String.IsNullOrEmpty(Host)) return null;

            HttpClient httpClient = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage.Method = Method;
            ExtractHeaders(httpClient);
            var uriBuilder = this.Uri != null ? new UriBuilder(Uri) : new UriBuilder
            {
                Host = Host,
                Path = BuildPath()
            };
            if (!String.IsNullOrEmpty(Scheme))
                uriBuilder.Scheme = Scheme;
            String _requestResult = String.Empty;
            if (Contents != null && Contents.Any())
            {
                if (Method == HttpMethod.Get)
                {
                    string query = BuildQueryString();
                    uriBuilder.Query = query;
                }
                else if (Method == HttpMethod.Post)
                {
                    if (!String.IsNullOrEmpty(Content))
                    {
                        requestMessage.Content = new StringContent(Content);
                    }
                    else if (Contents != null && Contents.Any())
                    {
                        requestMessage.Content = new FormUrlEncodedContent(Contents);
                    }
                }
            }
            requestMessage.RequestUri = new Uri(uriBuilder.Uri.ToString(), UriKind.Absolute);

            try
            {
                HttpResponseMessage responseMessage = await httpClient.SendAsync(requestMessage).ConfigureAwait(false);

                if (callBack != null)
                {
                    callBack(responseMessage);
                }

                if (responseMessage != null)
                {
                    responseMessage.EnsureSuccessStatusCode();
                    return await responseMessage.Content.ReadAsStreamAsync();
                }
            }
            catch
            {
                if (callBack != null)
                {
                    callBack(null);
                }
            }

            return null;
        }

        private void ExtractHeaders(HttpClient httpClient)
        {
            if (Headers != null && Headers.Any())
            {
                foreach (var header in Headers)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
        }

        private String BuildQueryString()
        {
            if (Contents == null || !Contents.Any()) return String.Empty;
            StringBuilder builder = new StringBuilder();
            foreach (var content in Contents)
            {
                builder.Append($"{ System.Net.WebUtility.HtmlEncode(content.Key)}={ System.Net.WebUtility.HtmlEncode(content.Value)}&");
            }
            String data = builder.ToString();

            return data.Substring(0, data.Length - 1);
        }

        private String BuildPath()
        {
            if (Queries == null || !Queries.Any())
                return Path;

            StringBuilder builder = new StringBuilder();
            foreach (var path in Queries)
            {
                builder.Append($"{path}/");
            }
            String data = builder.ToString();

            return String.Format("{0}/{1}", Path, data.Substring(0, data.Length - 1));
        }
    }
}
