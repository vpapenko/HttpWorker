[![NuGet](http://img.shields.io/nuget/v/HttpWorker.svg)](https://www.nuget.org/packages/HttpWorker/)

# HttpWorker
Library to create HTTP API clients.

HttpWorker provides processing of requests in background and managing of network failures.
It has statuses like `CountOfUnprocessedHttpCalls`, `NetworkNotAvailable`, `LongOperationInProcess`, `Working`.

# How to use.
* Directly by using of `ApiClientBase`.

  Use
`async AddGetCall`
`async AddPostCall`
`async AddDeleteCall`
`async AddPutCall`
to add http call to queue.
* Inheritance

  Inherit ApiClientBase to create cpecific API client like in [JSONPlaceholderTestAPI.cs](TestAPI/JSONPlaceholderTestAPI.cs)
