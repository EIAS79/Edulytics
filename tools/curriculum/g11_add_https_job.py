"""Add disposable authenticated HTTPS G11 probe workflow, no production or local tests."""
from pathlib import Path
p=Path(__file__).resolve().parents[2]/'.github/workflows/us-ccss-g11-stage-d.yml'
s=p.read_text(encoding='utf8')
assert '\n  authenticated-g11-120-https:' not in s
s+="""
  authenticated-g11-120-https:
    needs: structural-inventory
    runs-on: ubuntu-24.04
    timeout-minutes: 75
    services:
      postgres:
        image: postgres:18
        env:
          POSTGRES_DB: edulytics_clean_ci
          POSTGRES_USER: postgres
          POSTGRES_PASSWORD: postgres
        ports: ['5432:5432']
        options: >-
          --health-cmd="pg_isready -U postgres -d edulytics_clean_ci"
          --health-interval=5s --health-timeout=5s --health-retries=20
    env:
      ConnectionStrings__DefaultConnection: 'Host=127.0.0.1;Port=5432;Database=edulytics_clean_ci;Username=postgres;Password=postgres;SSL Mode=Disable'
      ConnectionStrings__MigrationConnection: 'Host=127.0.0.1;Port=5432;Database=edulytics_clean_ci;Username=postgres;Password=postgres;SSL Mode=Disable'
      EDULYTICS_CLEAN_DATABASE_CONNECTION: 'Host=127.0.0.1;Port=5432;Database=edulytics_clean_ci;Username=postgres;Password=postgres;SSL Mode=Disable'
      EDULYTICS_CLEAN_DATABASE_NAME: edulytics_clean_ci
      EDULYTICS_CLEAN_SEED_CONFIRM: I_CONFIRM_NEW_EMPTY_DATABASE
      EDULYTICS_CI_REHEARSAL: 'true'
      EDULYTICS_CI_REHEARSAL_PROGRAMME: US-CCSS
      EDULYTICS_CI_G11_ALG2: 'true'
      G11_EXPECTED_CANDIDATE_SHA: ${{ github.event.pull_request.head.sha || github.sha }}
      ASPNETCORE_ENVIRONMENT: Staging
      PGPASSWORD: postgres
      Edulytics__LessonContent__ReadFromJson: 'true'
      Edulytics__Deployment__CleanBootstrap: 'true'
      Edulytics__Deployment__RunStartupDataMaintenance: 'false'
      Edulytics__Deployment__SeedCurriculum: 'false'
      Edulytics__PresentationDemo__Provision: 'false'
      Edulytics__MeetingDemo__ResetAndSeed: 'false'
    steps:
      - uses: actions/checkout@v4
        with:
          ref: ${{ github.event.pull_request.head.sha || github.sha }}
          fetch-depth: 0
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '10.0.200'
      - uses: actions/setup-python@v5
        with:
          python-version: '3.12'
      - name: Build isolated disposable DB and G11 web
        shell: bash
        run: |
          set -euo pipefail
          python3 -m py_compile tools/phase29/g11_algebra2_https.py
          dotnet tool restore
          dotnet restore Edulytics.sln
          dotnet build Edulytics.sln --no-restore -c Release
          dotnet publish src/Edulytics.Web/Edulytics.Web.csproj -c Release --no-restore -o /tmp/g11-alg2-web
          dotnet ef database update --project src/Edulytics.Data/Edulytics.Data.csproj --startup-project src/Edulytics.Web/Edulytics.Web.csproj --context EdulyticsDbContext
          dotnet run --project tools/Edulytics.CleanSeed/Edulytics.CleanSeed.csproj
      - name: Authenticate G11 learner and verify all 120 actual rendered RichV2 lessons, cross-school isolation
        shell: bash
        env:
          RENDER_SERVICE_ID: ci-disposable
          Edulytics__Deployment__CleanBootstrap: 'false'
          Edulytics__MeetingDemo__ResetAndSeed: 'true'
          Edulytics__Deployment__RunStartupMigrations: 'false'
        run: |
          set -euo pipefail
          password="$(openssl rand -hex 24)Aa1!"
          echo "::add-mask::$password"
          export Edulytics__MeetingDemo__Password="$password"
          if ! dotnet run --project tools/Edulytics.CiFixtures/Edulytics.CiFixtures.csproj -- academic >/tmp/g11-fixtures.log 2>&1; then
            tail -n 150 /tmp/g11-fixtures.log
            exit 1
          fi
          grep -q 'MEETING_DEMO_SEED_COMPLETED' /tmp/g11-fixtures.log
          openssl req -x509 -newkey rsa:2048 -nodes -days 1             -keyout /tmp/g11-key.pem -out /tmp/phase-uat-cert.pem             -subj '/CN=localhost' -addext 'subjectAltName=DNS:localhost,IP:127.0.0.1' >/dev/null 2>&1
          openssl pkcs12 -export -out /tmp/g11-identity.pfx             -inkey /tmp/g11-key.pem -in /tmp/phase-uat-cert.pem             -passout pass:local-test-tls >/dev/null 2>&1
          export ASPNETCORE_URLS='https://127.0.0.1:5052'
          export ASPNETCORE_Kestrel__Certificates__Default__Path=/tmp/g11-identity.pfx
          export ASPNETCORE_Kestrel__Certificates__Default__Password=local-test-tls
          export Edulytics__MeetingDemo__ResetAndSeed='false'
          export ASPNETCORE_CONTENTROOT=/tmp/g11-alg2-web
          dotnet /tmp/g11-alg2-web/Edulytics.Web.dll >/tmp/g11-app.log 2>&1 &
          pid=$!
          trap 'kill "$pid" 2>/dev/null || true' EXIT
          ready=false
          for retry in $(seq 1 100); do
            if curl -fsS --cacert /tmp/phase-uat-cert.pem https://localhost:5052/health/ready >/dev/null; then ready=true; break; fi
            if ! kill -0 "$pid" 2>/dev/null; then tail -n 120 /tmp/g11-app.log; exit 1; fi
            sleep 3
          done
          if [[ "$ready" != true ]]; then tail -n 100 /tmp/g11-app.log; exit 1; fi
          python3 tools/phase29/g11_algebra2_https.py
      - uses: actions/upload-artifact@v4
        if: always()
        with:
          name: g11-algebra2-120-authenticated-https
          path: /tmp/g11-algebra2-120-authenticated-render.json
          if-no-files-found: warn
          retention-days: 14
"""
# Backslash escapes in multiline Python literal not intended to concatenate workflow lines.
s=s.replace(" -nodes -days 1             -keyout", " -nodes -days 1 \\\n            -keyout")
s=s.replace(" -out /tmp/phase-uat-cert.pem             -subj", " -out /tmp/phase-uat-cert.pem \\\n            -subj")
s=s.replace(" -out /tmp/g11-identity.pfx             -inkey", " -out /tmp/g11-identity.pfx \\\n            -inkey")
s=s.replace(" -in /tmp/phase-uat-cert.pem             -passout", " -in /tmp/phase-uat-cert.pem \\\n            -passout")
p.write_text(s,encoding='utf8')
print('G11_HTTPS_DISPOSABLE_WORKFLOW_JOB_AUTHORED_TESTS_NOT_LOCAL_RUN')
