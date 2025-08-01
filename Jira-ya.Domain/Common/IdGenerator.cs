namespace Jira_ya.Domain.Common
{
    public static class IdGenerator
    {
        public static Guid New() => Guid.NewGuid();
    }
}
