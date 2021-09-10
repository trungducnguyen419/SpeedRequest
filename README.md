# SpeedRequest
SpeedRequest will help you run requests quickly and smoothly.

## Donate
- **Paypal**: trungd419@gmail.com

## Contacts
**Facebook**: [trungducmcgamer](https://facebook.com/trungducmcgamer/)  
**E-Mail**: trungd419@gmail.com

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
catch (Exception ex) {
	// Unhandled exceptions
}
finally {
    // Cleanup in the end if initialized
    request?.Dispose();
}
```
### Send multipart requests with fields and files
Use this code:
```csharp
SpeedRequest request = new SpeedRequest();
var multipartContent = new MultipartContent();
multipartContent.AddString("login", "username");
multipartContent.AddString("password", "password");
multipartContent.AddFile(@"C:\hp.rar", "file1", "hp.rar");
string response = request.RequestUrl("https://example.com/", Method.POST, "application/x-www-form-urlencoded", multipartContent);
// Read response
```


