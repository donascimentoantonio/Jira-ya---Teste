namespace Jira_ya.Application.Common
{
    public static class ErrorHandler
    {
        public static Result<T> HandleException<T>(Exception ex, string errorMessage)
        {
            System.Diagnostics.Debug.WriteLine($"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {errorMessage}\nException: {ex}");
            
            return Result<T>.Fail(errorMessage);
        }
    }
}
