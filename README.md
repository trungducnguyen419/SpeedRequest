# SpeedRequest
SpeedRequest will help you run requests quickly and smoothly.

## Donate
- **Paypal**: trungd419@gmail.com - Nguyen Trung Duc
- **VietTinBank**: 104869915061 - Nguyen Trung Thanh
- **MBBank**: 9704229287539964 - Nguyen Trung Thanh
- **ViettelPay**: +84375724938 - Nguyen Trung Thanh
## Contacts
- **Facebook**: [trungducmcgamer](https://www.facebook.com/trungducmcgamer/)  
- **Instagram**: [trungducmcvn](https://www.instagram.com/trungducmcvn/)
- **Github**: [trungducmcgamer](https://github.com/trungducmcgamer/)
- **Youtube**: [trungducmcgamer](https://www.youtube.com/trungducmcgamer/)
- **Discord**: [TrungDucMC_VN #2707](#)
- **Zalo**: [+84375724938](#)
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
HttpRequest request = null;
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
string response = request.Post("https://example.com", Method.POST, "application/x-www-form-urlencoded", multipartContent).ToString();
```

Get page source:
```csharp
string response = request.Get("https://example.com").ToString();
```

Post data:
```csharp
string response = request.Post("https://example.com", Method.POST, "application/x-www-form-urlencoded", "login=username&password=password").ToString();
```

Get receive the message body of the response:
```csharp
request.IgnoreProtocolErrors = true;
```

Add Headers:
```csharp
request.AddHeaders("name", "value");
```

Add User-Agent:
```csharp
request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36";
```

Get response headers:
```csharp
var response = request.Get("https://example.com");
Headers[] headersResponse = response.HeadersResponse;
foreach (Headers headers in headersResponse)
{
    // concat your string or do what you want
    Console.WriteLine($"{headers.Name}: {headers.Value}");
}
```

Download a file:
```csharp
var response = request.Get("https://example.com");
response.ToFile("C:\\myDownloadedFile.zip");
```

Get Cookies:
```csharp
var response = request.Get("https://example.com");
Cookies[] cookieResponse = response.Cookies;
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
request.Proxy = HttpProxyAuthenticationClient.Parse("127.0.0.1", 8080, "username", "password");
```
