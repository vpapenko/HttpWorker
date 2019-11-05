# HttpWorker
Library to create HTTP API clients.

HttpWorker provides processing of requests in background and managing of network failures.
It has statuses like `CountOfUnprocessedHttpCalls`, `NetworkNotAvailable`, `LongOperationInProcess`, `Working`.

This library is published as nuget package.

# How to use.
* Derectly by using of `ApiClientBase`.

  Use
`async AddGetCall`
`async AddPostCall`
`async AddDeleteCall`
`async AddPutCall`
to add http call to queue.
* Inheritance

  Inherit ApiClientBase to create cpecific API client like in [JSONPlaceholderTestAPI.cs](TestAPI/JSONPlaceholderTestAPI.cs)
