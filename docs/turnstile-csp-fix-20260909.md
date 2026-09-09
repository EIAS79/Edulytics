# Turnstile CSP fix

Cloudflare Turnstile requires its script and iframe origin to be permitted by the site's Content Security Policy. The public contact forms already render the Turnstile widget and verify tokens server-side; this change allows the browser to load the Turnstile client resources from `https://challenges.cloudflare.com`.

Allowed directives:
- `script-src https://challenges.cloudflare.com`
- `frame-src https://challenges.cloudflare.com`
- `connect-src https://challenges.cloudflare.com`

The existing nonce-based CSP and all other security directives remain in place.
