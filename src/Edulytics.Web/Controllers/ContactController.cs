using System.ComponentModel.DataAnnotations;
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
    private static readonly HashSet<string> SupportedFormTypes =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "sales",
            "demo",
            "support",
            "general",
            "partnership"
        };

    private readonly SmtpEmailOptions _smtp;
    private readonly ILogger<ContactController> _logger;

    public ContactController(
        IOptions<SmtpEmailOptions> smtp,
        ILogger<ContactController> logger)
    {
        _smtp = smtp.Value;
        _logger = logger;
    }

    [HttpGet("")]
    public IActionResult Index() => View();

    [HttpGet("sales-enquiry")]
    public IActionResult SalesEnquiry() => View("Inquiry", "sales");

    [HttpGet("request-demo")]
    public IActionResult RequestDemo() => View("Inquiry", "demo");

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
