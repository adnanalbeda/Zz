namespace Zz
{
    using System.Text;
    using Microsoft.AspNetCore.Http;

    public static class HttpRequestBodyReaderExtension
    {
        public static async Task<string> BodyToStringAsync(this HttpRequest request)
        {
            request.EnableBuffering();

            var bodyStr = "";

            // Arguments: Stream, Encoding, detect encoding, buffer size
            // AND, the most important: keep stream opened
            using (
                StreamReader reader = new StreamReader(
                    request.Body,
                    Encoding.UTF8,
                    true,
                    1024,
                    true
                )
            )
            {
                bodyStr = await reader.ReadToEndAsync();
            }

            // Rewind, so the core is not lost when it looks at the body for the request
            request.Body.Position = 0;

            return bodyStr;
        }
    }
}
