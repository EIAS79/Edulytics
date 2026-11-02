namespace Edulytics.Tests.Acceptance;

public sealed class PublicStaticFrontDoorContractTests
{
    [Fact]
    public void FrontDoor_BuildsExactServerRenderedSnapshotsForAllLanguages()
    {
        var root = FindRoot();
        var build = File.ReadAllText(Path.Combine(
            root,
            "public-frontdoor/build.sh"));

        Assert.Contains(
            "ORIGIN=\"https://edulytics-4346.onrender.com\"",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "snapshot_home \"pl\" \"$OUT/pl/index.html\"",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "snapshot_home \"en\" \"$OUT/en/index.html\"",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "snapshot_home \"ar\" \"$OUT/ar/index.html\"",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "\"/account/login\"",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "\"/schools/overview\"",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "\"/contact/request-demo\"",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "\"/legal/privacy\"",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "snapshot_public_route",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "Edulytics.Culture=c=${language}|uic=${language}",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "bundle_files \"$OUT/css/public-site-v44.css\"",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "bundle_files \"$OUT/js/public-site-v45.js\"",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "bundle_files \"$OUT/js/public-content-v33.js\"",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "\"js/public-home-commercial-v11.js\"",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "\"js/public-contact-system-v28.js\"",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "\"js/public-content-pages-v27.js\"",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "cp -R \"$SRC/.\" \"$OUT/\"",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "<script src=\"/frontdoor.js\" defer></script>",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "canonical=\"${route}/\"",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "find \"$OUT\" -type f -name '*.html' -print0",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "STATIC_FALLBACK_ORIGIN=\"https://edulytics-public.onrender.com\"",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "SNAPSHOT_ORIGIN=\"$ORIGIN\"",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "SOURCE_MODE=\"static\"",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "Main application is unavailable; rebuilding from the last live static snapshots.",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "data-frontdoor-language-bootstrap",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "d.dataset.snapshotLanguage=d.lang||\"pl\"",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "localStorage.getItem(\"edulytics.frontdoor.language\")",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "\"$SNAPSHOT_ORIGIN/${language}/\"",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "\"$SNAPSHOT_ORIGIN$route/\"",
            build,
            StringComparison.Ordinal);
    }

    [Fact]
    public void FrontDoor_RuntimeOnlyAddsWakeupAndBackendRouting()
    {
        var root = FindRoot();
        var runtime = File.ReadAllText(Path.Combine(
            root,
            "public-frontdoor/frontdoor.js"));

        Assert.Contains(
            "https://edulytics-4346.onrender.com",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "/health/ready?frontdoor=",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "ensureBackendReady",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "installLanguageRouting",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "staticLanguageDestination",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "installBackendRouting",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "LIVE_PREFIX = '/__frontdoor-live'",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "hydrateFromLiveApplication",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "rewritePostFormsToLiveBridge",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "'/account/login'",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "url.origin === window.location.origin",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "withPublicCulture",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "target.searchParams.set('culture', currentLanguage);",
            runtime,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "isLoginRoute && currentLanguage === 'ar'",
            runtime,
            StringComparison.Ordinal);

        Assert.Contains(
            "window.location.assign(",
            runtime,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "normalizeBackendLinks",
            runtime,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "frontdoorBackend",
            runtime,
            StringComparison.Ordinal);

        Assert.DoesNotContain(
            "completeTitle:",
            runtime,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "frontdoor-hero-card",
            runtime,
            StringComparison.Ordinal);
    }

    [Fact]
    public void FrontDoor_PublicRoutesStayStaticAndRenderWakeupIsBackgroundOnly()
    {
        var root = FindRoot();
        var runtime = File.ReadAllText(Path.Combine(
            root,
            "public-frontdoor/frontdoor.js"));

        Assert.Contains(
            "const publicStaticRoutes = new Set([",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "'/product/learning-built-for-understanding'",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "'/contact'",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "return publicStaticRoutes.has(path);",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "READY_AT_KEY",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "READY_TTL_MS",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "KEEP_WARM_INTERVAL_MS",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "window.sessionStorage.setItem(READY_AT_KEY",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "warmBackendInBackground();",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "installKeepWarm();",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "window.localStorage.setItem(LANGUAGE_KEY, selectedLanguage)",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "window.localStorage.setItem(PUBLIC_LANGUAGE_KEY, selectedLanguage)",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "frontdoorTemporarilyDisabled",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "unlockInteractiveSnapshot();",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "canonicalStaticUrl",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "normalizeStaticLinks();",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "target.pathname = `${path}/`;",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "event.preventDefault();",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "return liveHydrationRoutes.has(path);",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "PUBLIC_LANGUAGE_KEY",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "window.localStorage.setItem(PUBLIC_LANGUAGE_KEY",
            runtime,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "preferredLanguage() !== 'pl'",
            runtime,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "if (liveHydrationRoutes.has(normalizePath(window.location.pathname))) {\n      showStatus('preparing');",
            runtime,
            StringComparison.Ordinal);
    }

    [Fact]
    public void FrontDoor_LiveBridge_IsRewrittenBeforeMvcRouting()
    {
        var root = FindRoot();
        var program = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Program.cs"));

        Assert.Contains(
            "\"/__frontdoor-live\"",
            program,
            StringComparison.Ordinal);
        Assert.Contains(
            "context.Request.Path.StartsWithSegments",
            program,
            StringComparison.Ordinal);
        Assert.Contains(
            "context.Request.Path =",
            program,
            StringComparison.Ordinal);
    }

    [Fact]
    public void ReadyEndpoint_AllowsCrossOriginFrontDoorPolling()
    {
        var root = FindRoot();
        var program = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Program.cs"));

        Assert.Contains(
            "\"/health/ready\"",
            program,
            StringComparison.Ordinal);
        Assert.Contains(
            "AccessControlAllowOrigin = \"*\"",
            program,
            StringComparison.Ordinal);
        Assert.Contains(
            "CacheControl = \"no-store\"",
            program,
            StringComparison.Ordinal);
    }

    [Fact]
    public void ProductionRouting_DoesNotInvokeSuspendedDemoServices()
    {
        var root = FindRoot();
        var program = File.ReadAllText(Path.Combine(
            root, "src/Edulytics.Web/Program.cs"));
        var login = File.ReadAllText(Path.Combine(
            root, "src/Edulytics.Web/Views/Account/Login.cshtml"));
        var bootstrap = File.ReadAllText(Path.Combine(
            root, "src/Edulytics.Web/Bootstrap/EdulyticsDatabaseBootstrapper.cs"));
        var publicBuild = File.ReadAllText(Path.Combine(
            root, "public-frontdoor/build.sh"));

        Assert.DoesNotContain("DemoSameOriginGateway", program, StringComparison.Ordinal);
        Assert.DoesNotContain("MeetingDemoProvisioner", program, StringComparison.Ordinal);
        Assert.DoesNotContain("PresentationDemoProvisioner", bootstrap, StringComparison.Ordinal);
        Assert.DoesNotContain("MeetingDemoProvisioner", bootstrap, StringComparison.Ordinal);
        Assert.DoesNotContain("unified-demo-login.js", login, StringComparison.Ordinal);
        Assert.Contains("Old cached HTML may retain the retired demo login interceptor.",
            publicBuild, StringComparison.Ordinal);
        Assert.Contains("ORIGIN=\"https://edulytics-4346.onrender.com\"", publicBuild, StringComparison.Ordinal);
    }

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
                return directory.FullName;

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Edulytics solution root not found.");
    }
}
