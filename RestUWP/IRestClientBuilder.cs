using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Imagine.Rest.UWP
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
        IRestClientBuilder AddParameter(String key, object value);
        IRestClientBuilder AddParameters(Dictionary<String, String> parameters);
        IRestClientBuilder AddQuery(object parameter);
        IRestClientBuilder AddQueries(IEnumerable<Object> queries);
        IRestClientBuilder SetQuery(params object[] queries);
        IRestClientBuilder AddHeader(String key, object value);
        RestClient Build();
    }
}
