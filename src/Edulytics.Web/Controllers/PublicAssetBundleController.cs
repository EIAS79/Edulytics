using System.Collections.Concurrent;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

[AllowAnonymous]
public sealed class PublicAssetBundleController(IWebHostEnvironment environment) : Controller
{
    private static readonly ConcurrentDictionary<string, string> Cache = new(StringComparer.Ordinal);

    private static readonly string[] CssFiles =
    [
        "css/round2-product-fixes.css",
        "css/public-home.css",
        "css/round4-ux-display.css",
        "css/public-home-commercial-v4.css",
        "css/public-home-commercial-v5.css",
        "css/public-home-commercial-v6.css",
        "css/public-home-commercial-v10.css",
        "css/public-home-commercial-v11.css",
        "css/public-home-commercial-v12.css",
        "css/public-home-commercial-v13.css",
        "css/public-home-commercial-v14.css",
        "css/public-home-commercial-v15.css",
        "css/public-home-commercial-v16.css",
        "css/public-home-commercial-v17.css",
        "css/public-home-philosophy-v23.css",
        "css/public-contact-v4.css",
        "css/public-contact-system-v28.css",
        "css/public-home-footer-v24.css",
        "css/public-home-navbar-v25.css",
        "css/public-understanding-v1.css",
        "css/public-content-pages-v27.css",
        "css/public-arabic-rtl-v29.css",
        "css/public-home-visual-contract-v32.css",
        "css/public-home-mascot-transparency-v35.css"
    ];

    private static readonly string[] JsFiles =
    [
        "js/public-home-commercial-v4.js",
        "js/public-home-commercial-v5.js",
        "js/public-home-commercial-v6.js",
        "js/public-home-commercial-v9.js",
        "js/public-home-commercial-v10.js",
        "js/public-home-commercial-v11.js",
        "js/public-home-commercial-v12.js",
        "js/public-home-commercial-v13.js",
        "js/public-home-commercial-v14.js",
        "js/public-home-commercial-v15.js",
        "js/public-home-cartoon-cleanup.js",
        "js/public-home-experience-v20.js",
        "js/public-home-curricula-v21.js",
        "js/public-home-ai-spotlight-v22.js",
        "js/public-site-routing-v27.js",
        "js/public-site-routing-v28.js",
        "js/public-understanding-v1.js",
        "js/public-contact-system-v28.js"
    ];

    private static readonly string[] ContentJsFiles =
    [
        "js/public-content-pages-v27-en.js",
        "js/public-content-pages-v27-pl.js",
        "js/public-content-pages-v27-ar.js",
        "js/public-content-pages-v27.js"
    ];

    [HttpGet("/css/public-site-v31.css")]
    [HttpGet("/css/public-site-v32.css")]
    [HttpGet("/css/public-site-v33.css")]
    [HttpGet("/css/public-site-v35.css")]
    [HttpGet("/css/public-site-v38.css")]
    [HttpGet("/css/public-site-v39.css")]
    [HttpGet("/css/public-site-v40.css")]
    [HttpGet("/css/public-site-v41.css")]
    [HttpGet("/css/public-site-v42.css")]
    [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Client)]
    public IActionResult Css() =>
        Bundle("public-css-v42", CssFiles, "text/css; charset=utf-8");

    [HttpGet("/js/public-site-v31.js")]
    [HttpGet("/js/public-site-v32.js")]
    [HttpGet("/js/public-site-v33.js")]
    [HttpGet("/js/public-site-v36.js")]
    [HttpGet("/js/public-site-v37.js")]
    [HttpGet("/js/public-site-v38.js")]
    [HttpGet("/js/public-site-v39.js")]
    [HttpGet("/js/public-site-v40.js")]
    [HttpGet("/js/public-site-v41.js")]
    [HttpGet("/js/public-site-v42.js")]
    [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Client)]
    public IActionResult JavaScript() =>
        Bundle("public-js-v42", JsFiles, "application/javascript; charset=utf-8");

    [HttpGet("/js/public-content-v31.js")]
    [HttpGet("/js/public-content-v32.js")]
    [HttpGet("/js/public-content-v33.js")]
    [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Client)]
    public IActionResult ContentJavaScript() =>
        Bundle("public-content-js-v33", ContentJsFiles, "application/javascript; charset=utf-8");

    private IActionResult Bundle(
        string cacheKey,
        IReadOnlyList<string> files,
        string contentType)
    {
        var content = Cache.GetOrAdd(
            cacheKey,
            _ => ReadBundle(files));

        Response.Headers.CacheControl = "public,max-age=86400";
        return Content(content, contentType, Encoding.UTF8);
    }

    private string ReadBundle(IReadOnlyList<string> files)
    {
        var webRoot = environment.WebRootPath;
        if (string.IsNullOrWhiteSpace(webRoot))
            throw new InvalidOperationException("The web root is not available.");

        var builder = new StringBuilder();
        foreach (var relativePath in files)
        {
            var fullPath = Path.Combine(
                webRoot,
                relativePath.Replace('/', Path.DirectorySeparatorChar));

            if (!System.IO.File.Exists(fullPath))
                throw new FileNotFoundException("A public bundle source file is missing.", relativePath);

            builder.AppendLine($"/* {relativePath} */");
            builder.AppendLine(System.IO.File.ReadAllText(fullPath));
            builder.AppendLine(";");
        }

        return builder.ToString();
    }
}
