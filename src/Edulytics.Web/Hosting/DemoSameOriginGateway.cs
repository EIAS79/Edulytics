using System.Net;
using Microsoft.AspNetCore.Http;

namespace Edulytics.Web.Hosting;

/// <summary>
/// Server-side, fixed-destination gateway for the permanent demo school tenant.
/// Requests remain on edulytiks.com; no browser redirect to Render is required.
/// Only Program.cs's Render-service-ID guard can invoke this gateway.
/// </summary>
internal static class DemoSameOriginGateway
{
    private const string Prefix = "/__frontdoor-live/demo";
    private const string DemoOrigin = "https://edulytics-schools-demo.onrender.com";

    private static readonly HttpClient Client = new(new SocketsHttpHandler
    {
        AllowAutoRedirect = false,
        UseCookies = false,
        AutomaticDecompression = DecompressionMethods.None,
        PooledConnectionLifetime = TimeSpan.FromMinutes(10)
    })
    {
        Timeout = Timeout.InfiniteTimeSpan
    };

    private static readonly HashSet<string> HopByHopHeaders =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "Host", "Connection", "Keep-Alive", "Proxy-Authenticate",
            "Proxy-Authorization", "TE", "Trailer", "Transfer-Encoding",
            "Upgrade", "Content-Length"
        };

    public static Task ForwardAsync(HttpContext context, PathString path) =>
        ForwardCoreAsync(context, path, useOriginalPath: false);

    public static Task ForwardOriginalPathAsync(HttpContext context) =>
        ForwardCoreAsync(context, context.Request.Path, useOriginalPath: true);

    private static async Task ForwardCoreAsync(
        HttpContext context, PathString path, bool useOriginalPath)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = StatusCodes.Status501NotImplemented;
            return;
        }

        var relative = path.HasValue ? path.ToUriComponent() : "/";
        var destination = new Uri(DemoOrigin + (useOriginalPath ? "" : Prefix) + relative +
                                  context.Request.QueryString.ToUriComponent());
        using var outbound = new HttpRequestMessage(
            new HttpMethod(context.Request.Method), destination);

        var hasBody = context.Request.ContentLength is > 0 ||
                      context.Request.Headers.ContainsKey("Transfer-Encoding");
        if (hasBody)
            outbound.Content = new StreamContent(context.Request.Body);

        foreach (var header in context.Request.Headers)
        {
            if (HopByHopHeaders.Contains(header.Key) ||
                header.Key.StartsWith("X-Forwarded-", StringComparison.OrdinalIgnoreCase))
                continue;

            if (!outbound.Headers.TryAddWithoutValidation(
                    header.Key, header.Value.ToArray()) &&
                outbound.Content is not null)
            {
                outbound.Content.Headers.TryAddWithoutValidation(
                    header.Key, header.Value.ToArray());
            }
        }

        outbound.Headers.Host = new Uri(DemoOrigin).Authority;
        // Do not pass the public host into the isolated upstream. Its own
        // authentication middleware must generate links against its own host;
        // those redirects are then rewritten into a same-origin path below.

        try
        {
            using var upstream = await Client.SendAsync(
                outbound, HttpCompletionOption.ResponseHeadersRead,
                context.RequestAborted);
            context.Response.StatusCode = (int)upstream.StatusCode;

            foreach (var header in upstream.Headers)
            {
                if (!HopByHopHeaders.Contains(header.Key))
                    context.Response.Headers[header.Key] =
                        header.Value.ToArray();
            }

            foreach (var header in upstream.Content.Headers)
            {
                if (!HopByHopHeaders.Contains(header.Key))
                    context.Response.Headers[header.Key] =
                        header.Value.ToArray();
            }

            // Force redirects to remain on the official, same-origin path.
            if (upstream.Headers.Location is { } location)
            {
                if (location.IsAbsoluteUri &&
                    string.Equals(location.Host,
                        new Uri(DemoOrigin).Host,
                        StringComparison.OrdinalIgnoreCase))
                {
                    context.Response.Headers.Location =
                        (useOriginalPath
                            ? location.AbsolutePath.StartsWith(
                                Prefix, StringComparison.OrdinalIgnoreCase)
                                ? location.AbsolutePath[Prefix.Length..]
                                : location.AbsolutePath
                            : location.AbsolutePath.StartsWith(
                                Prefix, StringComparison.OrdinalIgnoreCase)
                                ? location.AbsolutePath
                                : Prefix + location.AbsolutePath)
                        + location.Query + location.Fragment;
                }
                else if (!location.IsAbsoluteUri &&
                         location.OriginalString.StartsWith('/') &&
                         !location.OriginalString.StartsWith(Prefix,
                             StringComparison.OrdinalIgnoreCase))
                {
                    context.Response.Headers.Location =
                        (useOriginalPath ? "" : Prefix) + location.OriginalString;
                }
            }

            context.Response.Headers.Remove("transfer-encoding");
            await upstream.Content.CopyToAsync(
                context.Response.Body, context.RequestAborted);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            // The browser disconnected; the upstream operation is cancelled.
        }
        catch (HttpRequestException)
        {
            if (!context.Response.HasStarted)
                context.Response.StatusCode = StatusCodes.Status502BadGateway;
        }
    }
}
