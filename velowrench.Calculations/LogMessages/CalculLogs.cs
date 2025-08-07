using Microsoft.Extensions.Logging;
using velowrench.Utils.Enums;

namespace velowrench.Calculations.LogMessages;

internal static partial class CalculLogs
{
    [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "Calcul error: {message}")]
    public static partial void CalculInError(ILogger logger, string message);

    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "{calculName} is {state} at: {date}(UTC)")]
    public static partial void CalculStateChangedAt(ILogger logger, string calculName, ECalculState state, DateTime date);
}
