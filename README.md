# RestUWP
# Introdution
RestUWP is open sources library for Universal Windows Platfrom to work with Rest API.

Libraries:
  - `IRestClientBuilder.cs`
  - `RestClientBuilder`
  - `RestClient.cs`
# Using RestUWP
To use RestClient you have to provide required in formation:
  - `Scheme`: http or https
  - `Domain`: yourdomain.com

ex:
```cs
string scheme = "http";
string domain = "yourdomain.com";

RestClient client = new RestClientBuilder().
               SetScheme(scheme).
               SetHost(domain).
               Build();
               
string result = await client.RequestStringAsync();
```
# RestClientBuilder

It's use to create RestClient instance. You have to provide required information or extends request information via RestClientBuilder's methos before Build:

ex: 

```cs
string scheme = "http";
string domain = "harvard.com";
string path = "people/student"
string code = "123";

RestClient client = new RestClientBuilder().
               SetScheme(scheme).
               SetHost(domain).
               SetHost(path).
               AddPathParameter(code).
               Build();
               
string result = await client.RequestStringAsync();

// It's will return result from : http://harvard.com/people/student/123
```

# RestClientBuilder's Methods

It's implement the methods from `IRestClientBuilder` interface

- `SetHost`(String host): Your domain
- `SetScheme`(String Scheme): http or https
- `SetPath`(String path): your api path ex: School/Student/
- `SetMethod`(HttpMethod method): HttpMethod.Get, HttpMethod.Post.....
- `SetHeaders`(Dictionary<String, String> headers): set request headers 
- `SetContents`(Dictionary<String, String> contents): set request parameters
- `AddParameter`(String key, object value): add a parameter
- `AddParameters`(Dictionary<String, String> parameters): add dictionary<String, String> parameters
- `AddPathParameter`(object parameter): add slat ("/") + path parameter ex: /123
- `AddHeader`(String key, object value): add a request header
- RestClient `Build()`: Build RestClient with provided information

ex: 

```cs
// Post Method
string scheme = "http";
string domain = "harvard.com";
string path = "people/student/create"
HttpMethod method = HttpMethod.Post;
string code = "student_code_123";
string name = "Nguyen Pham";
string country = "Vietnam";

RestClient client = new RestClientBuilder().
               SetScheme(scheme).
               SetHost(domain).
               SetHost(path).
               SetMethod(method).
               SetParameter("name", name).
               SetParameter("code", code).
               SetParameter("country", country).
               Build();
                
var student = await client.ExcuteAsync<Student>();

// It's will return result from : http://harvard.com/people/student/create with provided infomations and then deserialize json result to Student object
```

# RestClient

Use to execute an request to Rest API 
# RestClient's Method
#### public async Task<T> `ExecuteAsync`<T>(Action<HttpResponseMessage> callBack = null)

Execute an request and parse json result to an object. To use it you have to provide class which you want to deserialize

ex:
```cs
// Get User Method
string scheme = "http";
string domain = "harvard.com";
string path = "people/student"
string userPath = "user"
HttpMethod method = HttpMethod.Get;
string code = "student_code_123";

RestClient client = new RestClientBuilder().
               SetScheme(scheme).
               SetHost(domain).
               SetHost(path).
               SetMethod(method).
               AddPathParameter(userPath).
               SetParameter("code", code).
               Build();
                
var student = await client.ExcuteAsync<Student>();

// It's will return json result from : http://harvard.com/people/student/user?code=student_code_123 and deserialize json result to Student object
```

### Others request methods:

#####  public async Task<String> `RequestStringAsync`(Action<HttpResponseMessage> callBack = null)
##### public async Task<byte[]> `RequestBytesAsync`(Action<HttpResponseMessage> callBack = null)
##### public async Task<Stream> `RequestStreamAsync`(Action<HttpResponseMessage> callBack = null)

It's very easy to use. If you have any question please contact me via pham.nguyen@hotmail.com

# Lisence 

The MIT License

Copyright (c) 2017 by Nguyen Pham

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software. 

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
