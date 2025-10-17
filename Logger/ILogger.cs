namespace LoggerClass
{
    /// <summary>
    /// Simple logger interface
    /// </summary>
    public interface ILogger
    {
        void Log(string Message, string module, Logger.LogLevel level);
    }
}