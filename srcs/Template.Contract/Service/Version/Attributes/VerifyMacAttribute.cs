using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Template.Contract.Common.Model;
using Template.Contract.Extensions;
using Template.Contract.Utility;

namespace Template.Contract.Service.Version.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class VerifyMacAttribute<T> : Attribute, IActionFilter where T : BaseModel
{
    private const string ClassName = nameof(VerifyMacAttribute<T>);
    public void OnActionExecuting(ActionExecutingContext context)
    {
        Log.Information("Verifying mac...".GeneratedLog(ClassName, LogEventLevel.Information));
        if (context.ActionArguments.Any())
        {
            foreach (var requestModel in context.ActionArguments)
            {
                if (requestModel.Value is not T model) continue;

                var mac = model.Mac;
                if (string.IsNullOrEmpty(mac)) continue;

                var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
                var secretKey = configuration.GetValue<string>("Config:SecretKey");

                if (!StringUtility.VerifyMac(model, secretKey, mac))
                {
                    continue;
                }

                Log.Information("Verified mac: Success!".GeneratedLog(ClassName, LogEventLevel.Information));
                return;
            }

        }
        Log.Warning("Verify mac fail, Unauthorized request".GeneratedLog(ClassName,LogEventLevel.Warning));
        context.Result = new UnauthorizedResult(); // Return 401 Unauthorized
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {

    }
}
