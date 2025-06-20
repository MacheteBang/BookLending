using System.Diagnostics;
using OpenTelemetry;

namespace MacheteBang.BookLending.Api.Telemetry;

internal sealed class HealthCheckTelemetryProcessor(ILogger<HealthCheckTelemetryProcessor> logger) : BaseProcessor<Activity>
{
    private static readonly string[] HealthCheckPaths = ["/_health", "/_ready"];
    private readonly ILogger<HealthCheckTelemetryProcessor> _logger = logger;

    public override void OnEnd(Activity activity)
    {
        base.OnEnd(activity);

        if (IsHealthCheckActivity(activity))
        {
            // Mark the activity to be excluded from export
            activity.IsAllDataRequested = false;
            activity.ActivityTraceFlags &= ~ActivityTraceFlags.Recorded;

            _logger.LogDebug("Filtered health check activity: {ActivityDisplayName}", activity.DisplayName);
        }
    }
    private static bool IsHealthCheckActivity(Activity activity)
    {
        if (HealthCheckPaths.Any(path => activity.DisplayName?.Contains(path) == true) ||
            HealthCheckPaths.Any(path => activity.OperationName?.Contains(path) == true))
        {
            return true;
        }

        foreach (var tag in activity.Tags)
        {
            string? value = tag.Value?.ToString();

            if (string.IsNullOrEmpty(value))
                continue;

            if ((tag.Key == "http.route" && HealthCheckPaths.Contains(value)) ||
                (tag.Key == "http.target" && HealthCheckPaths.Any(value.Contains)) ||
                (tag.Key == "http.url" && HealthCheckPaths.Any(value.Contains)) ||
                (tag.Key == "http.path" && HealthCheckPaths.Any(value.Contains)))
            {
                return true;
            }
        }

        return false;
    }
}
