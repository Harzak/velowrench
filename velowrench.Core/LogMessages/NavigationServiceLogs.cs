using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Core.Interfaces;

namespace velowrench.Core.LogMessages;

    internal static partial class NavigationServiceLogs
    {
    [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "Failed to initialize view model {viewModel}")]
    public static partial void ViewModelInitializationFailed(ILogger logger, string viewModel, Exception ex);
}
