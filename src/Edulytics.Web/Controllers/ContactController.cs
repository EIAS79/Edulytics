using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using Edulytics.Web.Email;
using Edulytics.Web.Support;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Edulytics.Web.Controllers;

[AllowAnonymous]
[Route("contact")]
public sealed class ContactController : Controller
{
    private const string TurnstileVerifyUrl =
        "https://challenges.cloudflare.com/turnstile/v0/siteverify";

    private const string TurnstileAction = "public_contact";

    private static readonly HashSet<string> AllowedTurnstileHostnames =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "edulytiks.com",
            "www.edulytiks.com"
        };

    private static readonly HashSet<string> SupportedFormTypes =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "sales",
            "demo",
            "support",
            "general",
            "partnership"
        };

    private static readonly HttpClient TurnstileHttpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(10)
    };

    private readonly SmtpEmailOptions _smtp;
    private readonly ILogger<ContactController> _logger;
    private readonly string _turnstileSiteKey;
    private readonly string _turnstileSecretKey;

    public ContactController(
        IOptions<SmtpEmailOptions> smtp,
        ILogger<ContactController> logger,
        IConfiguration configuration)
    {
        _smtp = smtp.Value;
        _logger = logger;
        _turnstileSiteKey = configuration["Turnstile:SiteKey"]?.Trim() ?? string.Empty;
        _turnstileSecretKey = configuration["Turnstile:SecretKey"]?.Trim() ?? string.Empty;
    }

    [HttpGet("")]
    public IActionResult Index() => View();

    [HttpGet("sales-enquiry")]
    public IActionResult SalesEnquiry() => OpenInquiry("sales");

    [HttpGet("request-demo")]
    public IActionResult RequestDemo() => OpenInquiry("demo");

    [HttpGet("support")]
    public IActionResult Support() => OpenInquiry("support");

    [HttpGet("message")]
    public IActionResult Message() => OpenInquiry("general");

    [HttpGet("help")]
    public IActionResult HelpAlias() => Redirect("/help");

    [HttpGet("/help")]
    public IActionResult Help() => View();

    [HttpPost("submit")]
    [ValidateAntiForgeryToken]
    [EnableRateLimiting("RequestDemo")]
    [RequestSizeLimit(32768)]
    public async Task<IActionResult> Submit(
        [FromForm] PublicContactFormInput input,
        [FromForm(Name = "cf-turnstile-response")] string? turnstileResponse,
        CancellationToken cancellationToken)
    {
        // Honeypot: real users never see or fill this field. Return a
        // success-shaped response so automated senders receive no signal.
        if (!string.IsNullOrWhiteSpace(input.Website))
        {
            return Ok(new { success = true });
        }

        var formType = input.FormType?.Trim().ToLowerInvariant() ?? string.Empty;
        if (!SupportedFormTypes.Contains(formType))
        {
            ModelState.AddModelError(
                nameof(input.FormType),
                "Unsupported contact form type.");
        }

        if (!ModelState.IsValid || HasUnsafeSingleLineInput(input))
        {
            return BadRequest(new
            {
                success = false,
                code = "validation",
                message = "Please check the form and try again."
            });
        }

        var visitorEmail = input.Email.Trim();
        if (!MailboxAddress.TryParse(visitorEmail, out var replyTo))
        {
            return BadRequest(new
            {
                success = false,
                code = "validation",
                message = "Please enter a valid email address."
            });
        }

        if (!string.IsNullOrWhiteSpace(input.Students)
            && (!int.TryParse(input.Students, out var students)
                || students < 0
                || students > 10_000_000))
        {
            return BadRequest(new
            {
                success = false,
                code = "validation",
                message = "Please check the student count and try again."
            });
        }

        if (string.IsNullOrWhiteSpace(turnstileResponse))
        {
            return BadRequest(new
            {
                success = false,
                code = "turnstile_required",
                message = "Please complete the security verification."
            });
        }

        var turnstileResult = await VerifyTurnstileAsync(
            turnstileResponse,
            cancellationToken);

        if (!turnstileResult.Success)
        {
            _logger.LogWarning(
                "Public contact Turnstile verification failed. FormType={FormType} Reason={Reason}",
                formType,
                turnstileResult.Reason);

            return BadRequest(new
            {
                success = false,
                code = "turnstile_failed",
                message = "Security verification failed. Please try again."
            });
        }

        if (!IsSmtpConfigurationValid())
        {
            _logger.LogWarning(
                "Public contact email delivery is unavailable because SMTP is not configured.");

            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                new
                {
                    success = false,
                    code = "delivery_unavailable",
                    message = "Message delivery is temporarily unavailable. Please try again later."
                });
        }

        using var connectorTimeout =
            CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        connectorTimeout.CancelAfter(
            TimeSpan.FromSeconds(_smtp.TimeoutSeconds));

        try
        {
            var message = new MimeMessage();
            message.From.Add(
                new MailboxAddress(
                    _smtp.FromName,
                    _smtp.FromAddress));

            // The destination is intentionally fixed server-side. A visitor
            // cannot alter the recipient through form data or browser tools.
            message.To.Add(
                MailboxAddress.Parse(
                    SupportContactOptions.Email));

            replyTo.Name =
                SafeSingleLine($"{input.FirstName} {input.LastName}");
            message.ReplyTo.Add(replyTo);

            message.Subject = BuildSubject(formType);
            message.Body = new BodyBuilder
            {
                // Plain text only. User-supplied markup is never rendered as
                // HTML in the message, removing an unnecessary injection path.
                TextBody = BuildBody(formType, input)
            }.ToMessageBody();

            using var client = new SmtpClient
            {
                Timeout = checked(_smtp.TimeoutSeconds * 1000)
            };

            await client.ConnectAsync(
                _smtp.Host,
                _smtp.Port,
                ResolveSecurity(),
                connectorTimeout.Token);

            if (!string.IsNullOrWhiteSpace(_smtp.Username))
            {
                await client.AuthenticateAsync(
                    _smtp.Username,
                    _smtp.Password,
                    connectorTimeout.Token);
            }

            await client.SendAsync(
                message,
                connectorTimeout.Token);

            await client.DisconnectAsync(
                true,
                connectorTimeout.Token);

            _logger.LogInformation(
                "Public contact form email sent. FormType={FormType}",
                formType);

            return Ok(new
            {
                success = true,
                code = "sent",
                message = "Your message has been sent successfully."
            });
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning(
                "Public contact email delivery timed out. FormType={FormType}",
                formType);

            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                new
                {
                    success = false,
                    code = "timeout",
                    message = "Message delivery timed out. Please try again."
                });
        }
        catch (Exception exception)
        {
            // Do not log visitor names, email addresses, message bodies, or
            // SMTP credentials. The exception type is sufficient for triage.
            _logger.LogWarning(
                "Public contact email delivery failed. FormType={FormType} ErrorType={ErrorType}",
                formType,
                exception.GetType().Name);

            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                new
                {
                    success = false,
                    code = "delivery_failed",
                    message = "We could not send your message right now. Please try again later."
                });
        }
    }

    private IActionResult OpenInquiry(string formType)
    {
        ViewData["TurnstileSiteKey"] = _turnstileSiteKey;
        return View("Inquiry", formType);
    }

    private async Task<TurnstileVerificationResult> VerifyTurnstileAsync(
        string responseToken,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_turnstileSecretKey))
        {
            return TurnstileVerificationResult.Fail("configuration_missing");
        }

        if (responseToken.Length > 4096)
        {
            return TurnstileVerificationResult.Fail("token_too_long");
        }

        using var content = new FormUrlEncodedContent(
            new Dictionary<string, string>
            {
                ["secret"] = _turnstileSecretKey,
                ["response"] = responseToken
            });

        try
        {
            using var response = await TurnstileHttpClient.PostAsync(
                TurnstileVerifyUrl,
                content,
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return TurnstileVerificationResult.Fail("siteverify_http_error");
            }

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var payload = await JsonSerializer.DeserializeAsync<TurnstileSiteverifyResponse>(
                stream,
                cancellationToken: cancellationToken);

            if (payload?.Success != true)
            {
                var reason = payload?.ErrorCodes is { Length: > 0 }
                    ? string.Join(',', payload.ErrorCodes)
                    : "verification_rejected";

                return TurnstileVerificationResult.Fail(reason);
            }

            if (string.IsNullOrWhiteSpace(payload.Hostname)
                || !AllowedTurnstileHostnames.Contains(payload.Hostname))
            {
                return TurnstileVerificationResult.Fail("hostname_mismatch");
            }

            if (!string.Equals(
                    payload.Action,
                    TurnstileAction,
                    StringComparison.Ordinal))
            {
                return TurnstileVerificationResult.Fail("action_mismatch");
            }

            return TurnstileVerificationResult.Ok();
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            _logger.LogWarning(
                "Cloudflare Turnstile Siteverify request failed. ErrorType={ErrorType}",
                exception.GetType().Name);

            return TurnstileVerificationResult.Fail("siteverify_unavailable");
        }
    }

    private bool IsSmtpConfigurationValid()
    {
        if (!_smtp.Enabled
            || string.IsNullOrWhiteSpace(_smtp.Host)
            || _smtp.Port <= 0
            || string.IsNullOrWhiteSpace(_smtp.FromAddress)
            || _smtp.TimeoutSeconds <= 0)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(_smtp.Username)
            && string.IsNullOrWhiteSpace(_smtp.Password))
        {
            return false;
        }

        return MailboxAddress.TryParse(_smtp.FromAddress, out _);
    }

    private SecureSocketOptions ResolveSecurity() =>
        _smtp.Security.Trim().ToLowerInvariant() switch
        {
            "none" => SecureSocketOptions.None,
            "sslonconnect" => SecureSocketOptions.SslOnConnect,
            "starttlswhenavailable" => SecureSocketOptions.StartTlsWhenAvailable,
            "auto" => SecureSocketOptions.Auto,
            _ => SecureSocketOptions.StartTls
        };

    private static string BuildSubject(string formType) =>
        formType switch
        {
            "demo" => "[Edulytics] Demo request",
            "support" => "[Edulytics] Support request",
            "partnership" => "[Edulytics] Partnership enquiry",
            "general" => "[Edulytics] Website enquiry",
            _ => "[Edulytics] Sales enquiry"
        };

    private static string BuildBody(
        string formType,
        PublicContactFormInput input)
    {
        var lines = new List<string>
        {
            "Edulytics website enquiry",
            "-------------------------",
            $"Type: {SafeSingleLine(formType)}",
            $"Name: {SafeSingleLine(input.FirstName)} {SafeSingleLine(input.LastName)}",
            $"Email: {SafeSingleLine(input.Email)}",
            $"School / organisation: {SafeSingleLine(input.Organisation)}",
            $"Role: {SafeSingleLine(input.Role)}"
        };

        if (!string.IsNullOrWhiteSpace(input.Country))
        {
            lines.Add($"Country: {SafeSingleLine(input.Country)}");
        }

        if (!string.IsNullOrWhiteSpace(input.Students))
        {
            lines.Add($"Approximate students: {SafeSingleLine(input.Students)}");
        }

        lines.Add(string.Empty);
        lines.Add("Message:");
        lines.Add(input.Message.Trim());
        lines.Add(string.Empty);
        lines.Add($"Submitted (UTC): {DateTimeOffset.UtcNow:O}");

        return string.Join(Environment.NewLine, lines);
    }

    private static bool HasUnsafeSingleLineInput(
        PublicContactFormInput input)
    {
        var singleLineValues = new[]
        {
            input.FormType,
            input.FirstName,
            input.LastName,
            input.Email,
            input.Organisation,
            input.Role,
            input.Country,
            input.Students
        };

        return singleLineValues.Any(
            value =>
                value?.Contains('\r') == true
                || value?.Contains('\n') == true);
    }

    private static string SafeSingleLine(string? value) =>
        (value ?? string.Empty)
            .Replace("\r", " ", StringComparison.Ordinal)
            .Replace("\n", " ", StringComparison.Ordinal)
            .Trim();

    private sealed class TurnstileSiteverifyResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; init; }

        [JsonPropertyName("hostname")]
        public string? Hostname { get; init; }

        [JsonPropertyName("action")]
        public string? Action { get; init; }

        [JsonPropertyName("error-codes")]
        public string[]? ErrorCodes { get; init; }
    }

    private sealed record TurnstileVerificationResult(bool Success, string Reason)
    {
        public static TurnstileVerificationResult Ok() => new(true, "ok");

        public static TurnstileVerificationResult Fail(string reason) => new(false, reason);
    }

    public sealed class PublicContactFormInput
    {
        [Required]
        [StringLength(24)]
        public string FormType { get; set; } = string.Empty;

        [Required]
        [StringLength(80, MinimumLength = 1)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(80, MinimumLength = 1)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(254)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(160, MinimumLength = 1)]
        public string Organisation { get; set; } = string.Empty;

        [Required]
        [StringLength(120, MinimumLength = 1)]
        public string Role { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Country { get; set; }

        [StringLength(12)]
        public string? Students { get; set; }

        [Required]
        [StringLength(3000, MinimumLength = 5)]
        public string Message { get; set; } = string.Empty;

        // Honeypot. It is visually hidden in the public form and must stay
        // empty. Automated form fillers commonly populate it.
        [StringLength(200)]
        public string? Website { get; set; }
    }
}
