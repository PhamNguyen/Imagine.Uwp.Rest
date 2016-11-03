using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RestUWP
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
        IRestClientBuilder AddParameter(String key, String value);
        IRestClientBuilder AddPathParameter(object parameter);
        IRestClientBuilder AddHeader(String key, String value);
        RestClient Build();
    }
}
