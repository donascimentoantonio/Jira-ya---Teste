namespace Jira_ya.Application.DTOs
{
    public class CreateRandomUsersRequest
    {
        public int Amount { get; set; }
        public string UserNameMask { get; set; }
    }
}
