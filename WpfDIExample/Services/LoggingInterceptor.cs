
using Microsoft.Extensions.Logging;
using System.Reflection;


namespace WpfDIExample.Services;

public class LoggingInterceptor<T> : DispatchProxy where T : class
{
    private T? _target;
    private ILogger? _logger;

    public static T Create(T target, ILogger logger)
    {
        var proxy = Create<T, LoggingInterceptor<T>>() as LoggingInterceptor<T>;
        proxy!._target = target;
        proxy._logger = logger;
        return (proxy as T)!;
    }

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        if (targetMethod == null || _target == null)
            return null;

        var logAttribute = targetMethod.GetCustomAttribute<LogExecutionAttribute>();

        if (logAttribute != null)
        {
            var methodName = targetMethod.Name;
            var className = _target.GetType().Name;
            var message = logAttribute.Message ?? methodName;

            _logger?.LogInformation(
                "AVANT - {ClassName}.{MethodName} | Message: {Message} | Args: [{Args}]",
                className,
                methodName,
                message,
                args != null ? string.Join(", ", args.Select(a => a?.ToString() ?? "null")) : "aucun"
            );

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var result = targetMethod.Invoke(_target, args);

                stopwatch.Stop();
                _logger?.LogInformation(
                    "APRÈS - {ClassName}.{MethodName} | Durée: {Duration}ms | Résultat: {Result}",
                    className,
                    methodName,
                    stopwatch.ElapsedMilliseconds,
                    result?.ToString() ?? "void"
                );

                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger?.LogError(
                    ex,
                    "ERREUR - {ClassName}.{MethodName} | Durée: {Duration}ms",
                    className,
                    methodName,
                    stopwatch.ElapsedMilliseconds
                );

                throw;
            }
        }
        else
        {
            return targetMethod.Invoke(_target, args);
        }
    }
}