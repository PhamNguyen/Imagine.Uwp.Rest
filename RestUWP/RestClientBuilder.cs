﻿// ******************************************************************
// Copyright (c) 2017 by Nguyen Pham. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Imagine.Uwp.Rest
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

        public IRestClientBuilder SetContent(string content)
        {
            Client.SetContent(content);
            return this;
        }

        public IRestClientBuilder SetPath(string path)
        {
            Client.Path = path;
            return this;
        }

        public IRestClientBuilder SetPort(int port)
        {
            Client.Port = port;
            return this;
        }

        public IRestClientBuilder SetMethod(HttpMethod method)
        {
            Client.Method = method;
            return this;
        }

        public IRestClientBuilder SetUri(Uri uri)
        {
            Client.Uri = uri;
            return this;
        }

        public IRestClientBuilder SetHeaders(Dictionary<string, string> headers)
        {
            if (headers == null)
                return this;
            Client.Headers = headers;
            return this;
        }

        public IRestClientBuilder SetHeaders(IEnumerable<KeyValuePair<string, string>> headers)
        {
            if (headers == null)
                return this;
            Client.Headers = headers.ToDictionary(p=>p.Key, p=>p.Value);
            return this;
        }

        public IRestClientBuilder SetContents(Dictionary<string, string> contents)
        {
            Client.SetContents(contents);
            return this;
        }

        public IRestClientBuilder AddParameter(string key, object value)
        {
            Client.AddParameter(key, value.ToString());
            return this;
        }

        public IRestClientBuilder AddQuery(object parameter)
        {
            Client.Queries.Add(parameter);
            return this;
        }

        public IRestClientBuilder AddHeader(string key, object value)
        {
            Client.AddHeader(key, value.ToString());
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

        public IRestClientBuilder AddParameters(Dictionary<String, String> parameters)
        {
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    Client.Contents[parameter.Key] = parameter.Value;
                }
            }
            return this;
        }

        public IRestClientBuilder SetQuery(params object[] queries)
        {
            Client.Queries = queries.ToList();
            return this;
        }

        public IRestClientBuilder AddQueries(IEnumerable<object> queries)
        {
            if (Client.Queries == null) Client.Queries = new List<object>();
            Client.Queries.AddRange(queries);
            return this;
        }

    }
}
