namespace api.Helpers;

public partial class ApiLogger<T>(ILogger<T> logger)
{
    private readonly ILogger<T> _logger = logger;

    public void Log(LogLevel level, string message, params object[] args)
    {
        if (_logger.IsEnabled(level))
        {
            // Passa direto para o logger que lida com placeholders estruturados ({Id}, {Data}, etc.)
            _logger.Log(level, message, args);
        }
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "{Message}")]
    private partial void LogInformation(string message);

    [LoggerMessage(Level = LogLevel.Warning, Message = "{Message}")]
    private partial void LogWarning(string message);

    [LoggerMessage(Level = LogLevel.Error, Message = "{Message}")]
    private partial void LogError(string message);

    // Com dados tipados
    public void LogWithData<TData>(LogLevel level, string message, TData data)
    {
        if (_logger.IsEnabled(level))
        {
            _logger.Log(level, "{Message}: {Data}", message, data);
        }
    }
}