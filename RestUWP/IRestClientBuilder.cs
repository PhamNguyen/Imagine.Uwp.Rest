// ******************************************************************
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
    public interface IRestClientBuilder
    {
        RestClient Client { get;}
        IRestClientBuilder SetHost(String host);
        IRestClientBuilder SetScheme(String Scheme);
        IRestClientBuilder SetPath(String path);
        IRestClientBuilder SetMethod(HttpMethod method);
        IRestClientBuilder SetHeaders(Dictionary<String, String> headers);
        IRestClientBuilder SetContents(Dictionary<String, String> contents);
        IRestClientBuilder SetContent(String content);
        IRestClientBuilder AddParameter(String key, object value);
        IRestClientBuilder AddParameters(Dictionary<String, String> parameters);
        IRestClientBuilder AddQuery(object parameter);
        IRestClientBuilder AddQueries(IEnumerable<Object> queries);
        IRestClientBuilder SetQuery(params object[] queries);
        IRestClientBuilder AddHeader(String key, object value);
        IRestClientBuilder SetHeaders(IEnumerable<KeyValuePair<string, string>> headers);
        IRestClientBuilder SetUri(Uri uri);
        IRestClientBuilder SetPort(int port);
        RestClient Build();
    }
}
