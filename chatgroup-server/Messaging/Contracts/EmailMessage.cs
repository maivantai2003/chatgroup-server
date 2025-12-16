namespace chatgroup_server.Messaging.Contracts
{
    public class EmailMessage
    {
        public string Name { get; init; } = "";
        public string Subject { get; init; } = "";
        public string Body { get; init; } = "";
        public List<string> ToEmails { get; init; } = new();
    }
}
