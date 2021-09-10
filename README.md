# SpeedRequest
SpeedRequest will help you run requests quickly and smoothly.

## Donate
- **Paypal**: trungd419@gmail.com

## Contacts
- **Facebook**: [trungducmcgamer](https://facebook.com/trungducmcgamer/)  
- **E-Mail**: trungd419@gmail.com

# Installation via [NuGet](https://www.nuget.org/)
```
Install-Package SpeedRequest
```
# Features
### HTTP Methods
- GET
- POST
- PATCH
- DELETE
- PUT
- OPTIONS
- HEAD
- CONNECT
- TRACE

# How to:
### Get started
Add in the beggining of file.
```csharp
using SpeedRequest;
```
And use one of this code templates:
```csharp
SpeedRequest request = null;
try {
    request = new SpeedRequest();
    // Do something 
}
catch (WebException ex) {
    // Http error handling
}
catch (Exception ex) {
    // Unhandled exceptions
}
```

Send multipart requests with fields and files:
```csharp
var multipartContent = new MultipartContent();
multipartContent.AddString("login", "username");
multipartContent.AddString("password", "password");
multipartContent.AddFile(@"C:\hp.rar", "file1", "hp.rar");
string response = request.RequestUrl("https://example.com", Method.POST, "application/x-www-form-urlencoded", multipartContent);
```

Get page source:
```csharp
string response = request.RequestUrl("https://example.com");
```

Post data:
```csharp
string response = request.RequestUrl("https://example.com", Method.POST, "application/x-www-form-urlencoded", "login=username&password=password");
```

Get receive the message body of the response:
```csharp
request.Requests().IgnoreProtocolErrors = true;
```

Get response headers:
```csharp
Headers[] headersResponse = request.Responses().HeadersResponse;
foreach (Headers headers in headersResponse)
{
    // concat your string or do what you want
    Console.WriteLine($"{headers.Name}: {headers.Value}");
}
```

Download a file:
```csharp
request.ToFile("https://example.com/file.zip", "C:\\myDownloadedFile.zip");
```

Get Cookies:
```csharp
string response = request.RequestUrl("https://example.com");
Cookies[] cookieResponse = request.Responses().Cookies;
foreach (Cookies cookie in cookieResponse)
{
    // concat your string or do what you want
    Console.WriteLine($"{cookie.Name}: {cookie.Value}");
}
```

Set proxy server:
```csharp
// Type: HTTP / HTTPS 
request.Proxy = HttpProxyClient.Parse("127.0.0.1:8080");
// Type: Socks4
request.Proxy = Socks4ProxyClient.Parse("127.0.0.1:9000");
// Type: Socks4a
request.Proxy = Socks4aProxyClient.Parse("127.0.0.1:9000");
// Type: Socks5
request.Proxy = Socks5ProxyClient.Parse("127.0.0.1:9000");
// Type: Proxy Authentication
request.Proxy = HttpProxyAuthenticationClient.Parse("127.0.0.1", "8080", "username", "password");
```
