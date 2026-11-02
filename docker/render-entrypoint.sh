#!/bin/sh
set -eu

if [ "$#" -gt 0 ]; then
    exec "$@"
fi

# Startup migrations are an explicit staging-only compatibility path.
# Production must run migrations in the host's pre-deploy phase using
# /app/phase27-predeploy.sh, then start the web process with this flag false.
RUN_STARTUP_MIGRATIONS="$(
    printf '%s' \
        "${Edulytics__Deployment__RunStartupMigrations:-false}" |
        tr '[:upper:]' '[:lower:]'
)"

case "$RUN_STARTUP_MIGRATIONS" in
    true)
        if [ -z "${ConnectionStrings__MigrationConnection:-}" ]; then
            echo "Startup migrations requested but migration connection is missing."
            exit 1
        fi

        echo "Applying pending database migrations before web startup..."

        ConnectionStrings__DefaultConnection="$ConnectionStrings__MigrationConnection" \
        EDULYTICS_CONNECTION_STRING="$ConnectionStrings__MigrationConnection" \
        /app/efbundle \
            --connection "$ConnectionStrings__MigrationConnection"

        echo "Database migration check completed."
        ;;
    false)
        echo "Startup migrations disabled; expecting controlled pre-deploy migration."
        ;;
    *)
        echo "Invalid Edulytics__Deployment__RunStartupMigrations value."
        exit 1
        ;;
esac

# Explicit one-time production clean bootstrap. The dedicated CLI performs its own
# fail-closed host/database checks, empty-data inspection and metadata-only seed.
# Keep this flag OFF after the initial verified database preparation.
RUN_CLEAN_SEED="$(printf '%s' "${Edulytics__Deployment__RunCleanSeedOnStartup:-false}" | tr '[:upper:]' '[:lower:]')"
case "$RUN_CLEAN_SEED" in
    true)
        if [ "${Edulytics__Deployment__CleanBootstrap:-false}" != "true" ] || \
           [ "${Edulytics__LessonContent__ReadFromJson:-false}" != "true" ] || \
           [ "$RUN_STARTUP_MIGRATIONS" != "true" ]; then
            echo "Clean seed requires clean bootstrap, JSON read and controlled startup migrations."
            exit 1
        fi
        echo "Checking and seeding only approved curriculum identities/metadata in independently verified empty database..."
        dotnet /app/clean-seed/Edulytics.CleanSeed.dll
        echo "Clean database curriculum metadata check completed."
        ;;
    false) ;;
    *) echo "Invalid Edulytics__Deployment__RunCleanSeedOnStartup value."; exit 1 ;;
esac

exec dotnet Edulytics.Web.dll \
    --urls "http://0.0.0.0:${PORT:-10000}"
