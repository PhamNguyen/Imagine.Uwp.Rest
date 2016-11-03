using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RestUWP
{
    public class RestClientBuilder : IRestClientBuilder
    {
        public RestClient Client
        {
            get;
        }

        public RestClientBuilder()
        {
            Client = new RestClient();
        }

        public IRestClientBuilder SetHost(string host)
        {
            Client.Host = host;
            return this;
        }

        public IRestClientBuilder SetPath(string path)
        {
            Client.Path = path;
            return this;
        }

        public IRestClientBuilder SetMethod(HttpMethod method)
        {
            Client.Method = method;
            return this;
        }

        public IRestClientBuilder SetHeaders(Dictionary<string, string> headers)
        {
            Client.Headers = headers;
            return this;
        }

        public IRestClientBuilder SetContents(Dictionary<string, string> contents)
        {
            Client.SetContents(contents);
            return this;
        }

        public IRestClientBuilder AddParameter(string key, string value)
        {
            Client.AddParameter(key, value);
            return this;
        }

        public IRestClientBuilder AddPathParameter(object parameter)
        {
            Client.PathParameters.Add(parameter);
            return this;
        }

        public IRestClientBuilder AddHeader(string key, string value)
        {
            Client.AddHeader(key, value);
            return this;
        }

        public RestClient Build()
        {
            return Client;
        }

        public IRestClientBuilder SetScheme(string scheme)
        {
            Client.Scheme = scheme;
            return this;
        }
       
    }
}
