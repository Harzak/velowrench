using Microsoft.Extensions.Logging;
using velowrench.Utils.Enums;

namespace velowrench.Calculations.LogMessages;

internal static partial class CalculatorLogs
{
    [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "Calculation error: {message}")]
    public static partial void CalculationInError(ILogger logger, string message);

    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "{calculatorName} is {state} at: {date}(UTC)")]
    public static partial void CalculationStateChangedAt(ILogger logger, string calculatorName, ECalculatorState state, DateTime date);
}
