namespace Zz.App.Core;

public interface IEmailSender
{
    public Task SendEmailAsync(
        IEnumerable<Recipient> contacts,
        string subject,
        BodyContent? body = null,
        IEnumerable<Attachment>? attachments = null,
        EmailContact? sender = null
    );

    public class BodyContent
    {
        public required BodyContentType Type { get; set; }
        public required string Content { get; set; }
    }

    public class Recipient
    {
        public required EmailContact Contact { get; set; }
        public ReceiveAsOption As { get; set; }
    }

    public class EmailContact
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
    }

    public enum ReceiveAsOption
    {
        Main,
        CC,
        BCC,
    }

    public enum BodyContentType
    {
        Html,
        Text,
    }

    public class Attachment
    {
        public required string MediaType { get; set; }
        public required string MediaSubType { get; set; }

        public required string FileName { get; set; }
        public required Stream Data { get; set; }
    }
}
