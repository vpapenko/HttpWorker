# HttpWorker
Library to create HTTP API clients.

HttpWorker provides processing of requests in background and managing of network failures.
It has statuses like LongOperationInProcess, Working, NetworkNotAvailable.

This library is published as nuget package.

# How to use.
* Derectly by using of `ApiClientBase`.

  Use
`async Task<T> AddGetCall<T>`
`async Task<T> AddPostCall<T>`
`async Task<T> AddDeleteCall<T>`
`async Task<T> AddPutCall<T>`
to add http call to queue.
* Inheritance

  Inherit ApiClientBase to create cpecific API client like in [TestAPI.cs](TestAPI/TestAPI.cs)
