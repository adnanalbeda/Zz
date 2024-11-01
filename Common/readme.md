# Common Project

## Result Status Code

First I went with using default http status codes,
then I found there are people using non-standard codes like 600s.

So I thought about it, and it sounds cool. How?

Think about `BadRequest`.

Asp sends BadRequest when it receives a literally bad request, like data 
shape or type is wrong or don't match request expected data.

This will return an error message with details different than what I need.

So I can use `600` status code, a non-standard code to tell clients that
data are not completely wrong. They have passed, they're just invalid.
`400` or `BadRequest` serves the same purpose, but this way, I can be sure
that response has specific type of error details that match my expected
invalid response `result` data.

Or like `404`, Asp returns 404 if url is not found.
But what if it does exist, but the processing cannot find result data?
Using `604` solves this issue especially for `put` methods.

Same as `InternalError`. Servers returning this error usually means that
server is dead for a reason or fatally failed in processing request.
But what if an error occurred only in a specific area, which didn't 
fail the whole app. Thus, code `700` can be used to indicate request has 
reached processing area, but failed while doing so, and this only 
affects this request.

This way, Api also avoid some unintended access if it's private, and only
services related to it can interpret the custom results and status codes.

### Zz Custom Status Code Standard

While using them might be good for detailed result, a second thought
would say not being too specific is better for security.

No, fuck it. Custom code it is:

####  Bad Processing Result : (SC+200)

- (`600`) Invalid Data. : Equivalent to (`400`)`BadRequest`
- (`600`) Invalid Data. : Equivalent to (`400`)`BadRequest`
- (`604`) Item(s) not found. : Equivalent to (`404`)`NotFound` 
- (`622`) Unprocessable Data. : Equivalent to (`422`)`UnprocessableEntity`
- (`629`) Too many retries. : Equivalent to (`429`)`TooManyRequests` 
- (`700`) Failed : Equivalent to (`500`)`InternalError`
