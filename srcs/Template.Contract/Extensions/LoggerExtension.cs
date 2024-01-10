using System.Net;
using Newtonsoft.Json;
using Serilog.Events;
using Template.Contract.Common.Model;

namespace Template.Contract.Extensions;

public static class LoggerExtension
{
    public static string GeneratedLog(this string messageLog, string serviceName, LogEventLevel logEventLevel)
    {
        var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        var ipAddress = ipHostInfo.AddressList[0];
        var logModel = new LogModel
        {
            FullData = messageLog,
            Timestamp = DateTime.UtcNow.ToUnixTimeMilliseconds(),
            SourceIp = ipAddress.ToString(),
            ServiceName = serviceName,
            Level = logEventLevel.ToString(),
            CustomTimestamp = DateTime.UtcNow.AddHours(7).ToUnixTimeMilliseconds()
        };

        return JsonConvert.SerializeObject(logModel);
    }
}
